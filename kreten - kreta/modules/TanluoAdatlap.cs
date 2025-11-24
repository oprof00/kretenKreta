using JWT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Enumeration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace kretenKreta.modules
{

    internal class TanluoAdatlap
    {
        private static string nev;
        private static string intezmenynev;

        public static string getAdatlap(string access_token, string institute_code)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri($"https://{institute_code}.e-kreta.hu/ellenorzo/v3/sajat/TanuloAdatlap");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "hu.ekreta.student/5.7.0 (SM-A556B; Android 14; SDK 34)");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            try
            {
                var response = client.GetAsync(client.BaseAddress).Result;
                string jsoncontent = response.Content.ReadAsStringAsync().Result;

                using (var jsonDoc = JsonDocument.Parse(jsoncontent))
                {
                    var root = jsonDoc.RootElement;

                    nev = root.GetProperty("Nev").GetString();
                    intezmenynev = root.GetProperty("IntezmenyNev").GetString();
                }
                if (nev.Contains("invalid_grant"))
                {
                    return "Hiba: Érvénytelen hozzáférés. Kérlek jelentkezz be újra.";
                }
                else
                {
                    return nev + ", " + intezmenynev;
                }

                //return $"Üdv, {nev}!";
                //return "Hiba alma sadfdaf sda ds fsda f";
            }
            catch (Exception ex)
            {
                return $"Hiba az adatlap lekérésekor: {ex.Message}";
            }
        }
    }
}
