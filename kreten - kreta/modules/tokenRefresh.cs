using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kretenKreta.modules
{
    internal class tokenRefresh
    {
        public static string refresh_token;
        public static void tokenRefreshing(string refresh_token, string institute_code)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri($"https://idp.e-kreta.hu/connect/token");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "hu.ekreta.student/5.7.0 (SM-A556B; Android 14; SDK 34)");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apiKey", "21ff6c25-d1da-4a68-a811-c881a6057463");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            var data = new StringContent(
                $"client_id=kreta-ellenorzo-student-mobile-android&grant_type=refresh_token&refresh_token={refresh_token}&scope=openid email offline_access kreta-ellenorzo-webapi.public&institute_code={institute_code}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            try
            {
                var response = client.PostAsync(client.BaseAddress, data).Result;
                string jsoncontent = response.Content.ReadAsStringAsync().Result;

                //MessageBox.Show(jsoncontent, "token");

                File.WriteAllText(@"C:\kreten\tokens.json", jsoncontent);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a token frissítésekor:\n{ex.Message}");
            }
        }
    }
}
