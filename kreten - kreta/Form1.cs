using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.IO;
using Microsoft.Web.WebView2.Core.Raw;

namespace kretenKreta
{
    public partial class Form1 : Form
    {
        private string codeVerifier;
        private const string clientId = "kreta-ellenorzo-student-mobile-android";
        private const string redirectUri = "https://mobil.e-kreta.hu/ellenorzo-student/prod/oauthredirect";
        private const string tokenUrl = "https://idp.e-kreta.hu/connect/token";
        private string schoolCode;
        private string tokens;

        public Form1()
        {
            InitializeComponent();
            webView21.NavigationCompleted += WebView21_NavigationCompleted;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartLogin("premontrei-keszthely");
        }

        private async void StartLogin(string schoolCode)
        {
            codeVerifier = GenerateCodeVerifier();
            string codeChallenge = GenerateCodeChallenge(codeVerifier);
            string state = GenerateRandomState();

            string loginUrl =
                $"https://idp.e-kreta.hu/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback" +
                $"%3Fredirect_uri%3D{Uri.EscapeDataString(redirectUri)}" +
                $"%26client_id%3D{clientId}" +
                $"%26response_type%3Dcode" +
                $"%26scope%3Dopenid%2520email%2520offline_access%2520kreta-ellenorzo-webapi.public" +
                $"%26code_challenge%3D{codeChallenge}" +
                $"%26code_challenge_method%3DS256" +
                $"%26state%3D{state}" +
                $"%26nonce%3D{state}" +
                $"%26institute_code%3D{schoolCode}";

            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(loginUrl);
        }

        private async void WebView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            string currentUrl = webView21.Source.ToString();

            if (currentUrl.StartsWith(redirectUri))
            {
                // Az auth code kinyerése az URL-bõl
                var uri = new Uri(currentUrl);
                var query = HttpUtility.ParseQueryString(uri.Query);
                string code = query["code"];


                if (!string.IsNullOrEmpty(code))
                {
                    //MessageBox.Show("Auth code: " + code);
                    await GetAccessTokenAsync(code);
                }
                else
                {
                    MessageBox.Show("Nem találtam authorization code-ot!");
                }
            }
        }

        private async System.Threading.Tasks.Task GetAccessTokenAsync(string code)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(
                    $"code={code}&code_verifier={codeVerifier}&redirect_uri={Uri.EscapeDataString(redirectUri)}&client_id={clientId}&grant_type=authorization_code",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded"
                );

                client.DefaultRequestHeaders.Add("User-Agent", "hu.ekreta.student/5.7.0 (SM-A556B; Android 14; SDK 34)");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await client.PostAsync(tokenUrl, data);
                var content = await response.Content.ReadAsStringAsync();

                File.WriteAllText(@"C:\kreten\tokens.json", content);


                if (response.IsSuccessStatusCode)
                {
                    //MessageBox.Show("Siker! Token: " + content);
                    WelcomeForm welcomeForm = new WelcomeForm(schoolCode);
                    welcomeForm.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show($"Hiba történt: {response.StatusCode}\n\n{content}");
                }
            }
        }

        // PKCE utilok
        private static string GenerateCodeVerifier()
        {
            var random = new byte[32];
            RandomNumberGenerator.Fill(random);
            return Base64UrlEncode(random);
        }

        private static string GenerateCodeChallenge(string verifier)
        {
            var bytes = Encoding.ASCII.GetBytes(verifier);
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(bytes);
                return Base64UrlEncode(hash);
            }
        }

        private static string GenerateRandomState()
        {
            var bytes = new byte[16];
            RandomNumberGenerator.Fill(bytes);
            return Base64UrlEncode(bytes);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            schoolCode = Interaction.InputBox("Kérem az iskola kódját PONTOSAN adja meg, különben késõbb problémákba ütközhet: \n (pl. premontrei-keszthely)", "Iskola kód", "premontrei-keszthely");
            if (schoolCode == "")
            {
                MessageBox.Show("Nincs iskola kód megadva, kilépek.", "HIBA!!", MessageBoxButtons.OK);
                Application.Exit();
            }
            else
            {
                if (!Directory.Exists(@"C:\kreten"))
                {
                    Directory.CreateDirectory(@"C:\kreten");
                    StartLogin(schoolCode);
                }
                else
                {
                    if (!File.Exists(@"C:\kreten\tokens.json"))
                    {
                        StartLogin(schoolCode);
                    }
                    else
                    {
                        if (File.ReadAllText(@"C:\kreten\tokens.json") == "")
                        {
                            StartLogin(schoolCode);
                        }
                        else
                        {
                            WelcomeForm welcomeForm = new WelcomeForm(schoolCode);
                            Form1 form1 = this;
                            //form1.Hide();
                            welcomeForm.ShowDialog();
                            //form1.Hide();
                        }
                    }
                    //StartLogin(schoolCode);
                    //WelcomeForm welcomeForm = new WelcomeForm(schoolCode);
                    //welcomeForm.Show();
                    //this.Close();
                }

            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
