using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace kretenKreta.modules
{
    internal class Atlag
    {
        public static string getAtlag(string access_token, string institute_code)
        {
            var simaJegy = new List<string>();
            var tzJegy = new List<string>();

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri($"https://{institute_code}.e-kreta.hu/ellenorzo/v3/sajat/Ertekelesek");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            
            try
            {
                var response = client.GetAsync(client.BaseAddress).Result;
                string jsoncontent = response.Content.ReadAsStringAsync().Result;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<Jegy> grades = JsonSerializer.Deserialize<List<Jegy>>(jsoncontent, options);


                foreach (var grade in grades)
                {
                    string jegy = !string.IsNullOrEmpty(grade.SzovegesErtek)
                        ? grade.SzamErtek.ToString()
                        : grade.ErtekeloTanarNeve.ToString();

                        string formattedJegy = $"{grade.Tantargy.Nev}: {jegy} -( {grade.Mod.Leiras} ) |--{grade.ErtekeloTanarNeve}--|";
                    if (grade.Mod.Leiras.Contains("témazáró"))
                    {
                        string jegyStr = Regex.Match(formattedJegy, @"\d+").Value;
                        tzJegy.Add(jegyStr);

                    }
                    else
                    {
                        string jegyStr = Regex.Match(formattedJegy, @"\d+").Value;
                        simaJegy.Add(jegyStr);
                    }

                }
                double osszegSima = 0;
                foreach (var item in simaJegy)
                {
                    osszegSima += int.Parse(item);
                }
                double osszegTZ = 0;
                foreach (var item in tzJegy)
                {
                    osszegTZ += int.Parse(item);
                }
                osszegTZ = osszegTZ * 2;
                double osszeg = osszegSima + osszegTZ;
                double darab = simaJegy.Count + (tzJegy.Count *2);
                double atlag = osszeg / darab;

                return $"Átlag: {atlag.ToString("0.00")}";

            }
            catch (HttpIOException ex)
            {
                return ex.Message;
            }
        }
    }
}
