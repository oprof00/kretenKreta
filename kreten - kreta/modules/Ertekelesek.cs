//using Newtonsoft.Json;
using kretenKreta.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        public Tantargy Tantargy { get; set; }
        public string Leiras { get; set; }
    }
    public class Jegy
    {
        public Tantargy Tantargy { get; set; }
        public Mod Mod { get; set; }
        public string SzovegesErtek { get; set; }
        public double SzamErtek { get; set; }
        public string ErtekeloTanarNeve { get; set; }
    }
    internal class Ertekelesek
    {
        private static string jelleg;
        private static string szamertek;

        public static string GetErtekelesek(string access_token, string institute_code)
        {
            var lista = new List<string>();

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri($"https://{institute_code}.e-kreta.hu/ellenorzo/v3/sajat/Ertekelesek");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            //asd

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

                    lista.Add($"{grade.Tantargy.Nev}: {jegy} -( {grade.Mod.Leiras} ) |--{grade.ErtekeloTanarNeve}--|");
                    
                }
                return string.Join("\n", lista);

            }
            catch (HttpIOException ex)
            {
                return ex.Message;
            }
            
        }
    }
}
