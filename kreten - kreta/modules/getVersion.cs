using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kretenKreta.modules
{
    internal class getVersion
    {
        private static string version;
        public static string verzioKeres()
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://raw.githubusercontent.com/oprof00/kreten/refs/heads/main/version.txt");

            try
            {
                var response = client.GetAsync(client.BaseAddress).Result;
                version = response.Content.ReadAsStringAsync().Result;
                return version.ToString();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;

            }
        }
    }
}