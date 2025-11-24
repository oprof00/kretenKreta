using System;
using System.Windows.Forms;
using kretenKreta;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using JWT;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Enumeration;
using System.Windows.Forms.VisualStyles;
using kretenKreta.modules;

namespace kretenKreta
{
    public partial class WelcomeForm : Form
    {
        private string tokens;
        private string institute_code;

        private string id_token;
        private string access_token;
        private string refresh_token;
        private string token_type;
        private int expires_in;
        private static string token_file_path = @"C:\kreten";
        private static string token_file = "tokens.json";
        private static string tokenFiles = Path.Combine(token_file_path, token_file);

        public WelcomeForm(string schoolCode)
        {
            institute_code = schoolCode;
            InitializeComponent();

        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            if (File.ReadAllText(@"C:\kreten\tokens.json") == "")
            {
                try
                {
                    File.WriteAllText(@"C:\kreten\tokens.json", tokens);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Hiba a tokenek mentésekor:\n{ex.Message}");
                }
            }




            try
            {
                string jsonContent = File.ReadAllText(tokenFiles);
                using (var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonContent))
                {
                    var root = jsonDoc.RootElement;

                    id_token = root.GetProperty("id_token").GetString();
                    access_token = root.GetProperty("access_token").GetString();
                    refresh_token = root.GetProperty("refresh_token").GetString();
                    token_type = root.GetProperty("token_type").GetString();
                    expires_in = root.GetProperty("expires_in").GetInt32();




                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"asd Hiba a JSON feldolgozásakor:\n{ex.Message}");
            }
            if (TanluoAdatlap.getAdatlap(access_token, institute_code).StartsWith("Hiba"))
            {
                MessageBox.Show("Hiba történt. \n A Kretén most bezáródik. Kérlek nyisd meg újra", "Hiba");
                try
                {
                    File.Delete(tokenFiles);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Hiba a kijelentkezéskor:\n{ex.Message}");
                }
            }
            else
            {
                //nev es suli
                nev.Text = TanluoAdatlap.getAdatlap(access_token, institute_code);
                //verzio lekeres
                var verzioKer = getVersion.verzioKeres;
                string verzio = verzioKer();
                lblVersion.Text = $"Verzió: {verzio}";

                //jegyek
                var jegyekString = Ertekelesek.GetErtekelesek(access_token, institute_code);
                var jegyekLista = jegyekString.Split('\n');
                listJegy.Items.Clear();
                listJegy.Items.AddRange(jegyekLista);
            }





        }
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("A kretén most bezáródik és kijelentkezteti a rendszerből.\nKérlek indítsd újra.", "Kilépés");
                File.Delete(tokenFiles);
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Hiba a kijelentkezéskor:\n{ex.Message}");
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listJegy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

    }
}
