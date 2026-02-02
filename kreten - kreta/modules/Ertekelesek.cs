using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kretenKreta.modules
{
    public class Tantargy
    {
        public string Nev { get; set; }
    }

    public class Mod
    {
        public string Leiras { get; set; }
        public string Nev { get; set; }
    }

    public class Jegy
    {
        public Tantargy Tantargy { get; set; }
        public Mod Mod { get; set; }
        public string SzovegesErtek { get; set; }
        public double SzamErtek { get; set; }
        public int? SulySzazalekErteke { get; set; }
        public string ErtekeloTanarNeve { get; set; }
        public string RogzitesDatuma { get; set; }
    }

    internal class Ertekelesek
    {
        public static string GetErtekelesek(string access_token, string institute_code)
        {
            List<string> lista = new List<string>();

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

                    if (grades == null) return "Nem érkeztek jegyek.";

                    foreach (var grade in grades)
                    {
                        int szazalek = grade.SulySzazalekErteke ?? 0;

                        if (szazalek == 0)
                        {
                            continue;
                        }

                        string tantargyNeve = grade.Tantargy?.Nev ?? "Ismeretlen tárgy";
                        string modLeiras = grade.Mod?.Leiras ?? "Egyéb";
                        string tanar = grade.ErtekeloTanarNeve ?? "Ismeretlen tanár";

                        string jegyMegjelenites = grade.SzamErtek != 0
                            ? grade.SzamErtek.ToString()
                            : grade.SzovegesErtek;

                        lista.Add($"{tantargyNeve}: {jegyMegjelenites} ({szazalek}%) - ({modLeiras}) |-- {tanar} --|");
                    }

                    return string.Join("\n", lista);
                }
                catch (HttpRequestException ex)
                {
                    return $"Hálózati hiba: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Egyéb hiba: {ex.Message}";
                }
            }
        }
    }
}