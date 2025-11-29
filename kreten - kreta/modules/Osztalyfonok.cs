using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kretenKreta.modules
{
    internal class Osztalyfonok
    {
        public static string getOfo(string access_token, string institute_code)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri($"https://{institute_code}.e-kreta.hu/ellenorzo/v3/felhasznalok/Alkalmazottak/Tanarok/Osztalyfonokok?Uids=name");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "hu.ekreta.student/5.7.0 (SM-A556B; Android 14; SDK 34)");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);


            try
            {
                var response = client.GetAsync(client.BaseAddress).Result;
                string jsoncontent = response.Content.ReadAsStringAsync().Result;

                /*using (var jsonDoc = JsonDocument.Parse(jsoncontent))
                {
                    var root = jsonDoc.RootElement;

                    ofo = root.GetProperty("Nev").GetString();
                    intezmenynev = root.GetProperty("IntezmenyNev").GetString();
                }*/
                return jsoncontent;


            }
            catch (Exception ex)
            {
                return $"Hiba a token frissítésekor:\n{ex.Message}";
            }
        }
    }
}
