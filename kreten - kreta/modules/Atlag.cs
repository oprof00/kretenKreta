using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace kretenKreta.modules
{
    internal class Atlag
    {
        public static string getAtlag(string access_token, string institute_code)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri($"https://{institute_code}.e-kreta.hu/ellenorzo/v3/sajat/Ertekelesek");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                try
                {
                    var response = client.GetAsync(client.BaseAddress).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return $"Hiba történt: {response.StatusCode}";
                    }

                    string jsoncontent = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    };

                    List<Jegy> grades = JsonSerializer.Deserialize<List<Jegy>>(jsoncontent, options);

                    if (grades == null || grades.Count == 0) return "Nincsenek jegyek.";

                    StringBuilder kimenet = new StringBuilder();
                    List<double> tantargyAtlagok = new List<double>(); // Ebbe gyűjtjük az átlagokat a végleges átlaghoz

                    var tantargyak = grades.GroupBy(g => g.Tantargy?.Nev ?? "Egyéb");

                    //kimenet.AppendLine("--- TANTÁRGYI ÁTLAGOK ---");

                    foreach (var csoport in tantargyak)
                    {
                        double osszSulyozottErtek = 0;
                        double osszSuly = 0;

                        foreach (var jegy in csoport)
                        {
                            if (jegy.SzamErtek == 0 || (jegy.SulySzazalekErteke ?? 0) == 0)
                                continue;

                            double suly = (jegy.SulySzazalekErteke ?? 100) / 100.0;

                            osszSulyozottErtek += jegy.SzamErtek * suly;
                            osszSuly += suly;
                        }

                        if (osszSuly > 0)
                        {
                            double atlag = Math.Round(osszSulyozottErtek / osszSuly, 2);
                            tantargyAtlagok.Add(atlag);

                            //kimenet.AppendLine($"{csoport.Key}: {atlag:F2}");
                        }
                    }

                    if (tantargyAtlagok.Count > 0)
                    {
                        double vegsoAtlag = Math.Round(tantargyAtlagok.Average(), 2);
                        //kimenet.AppendLine("\n-------------------------");
                        //kimenet.AppendLine($"ÖSSZESÍTETT ÁTLAG: {vegsoAtlag:F2}");
                        kimenet.AppendLine(vegsoAtlag.ToString("F2"));
                    }

                    return kimenet.ToString();
                }
                catch (Exception ex)
                {
                    return $"Hiba az átlagszámítás közben: {ex.Message}";
                }
            }
        }
    }
}