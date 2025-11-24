using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace kretenKreta.modules
{
    public class Feljegyzes
    {
        [JsonPropertyName("Tartalom")]
        public string? content { get; set; }
        [JsonPropertyName("TartalomFormazott")]
        public string? contentFormatted { get; set; }
        [JsonPropertyName("KeszitesDatuma")]
        public string? creatingTimeAsString { get; set; }
        [JsonPropertyName("Datum")]
        public string? dateAsString { get; set; }
        [JsonPropertyName("OsztalyCsoport")]
        public string? seenByTutelaryAsString { get; set; }
        [JsonPropertyName("KeszitoTanarNeve")]
        public string? teacher { get; set; }
        [JsonPropertyName("Cim")]
        public string? title { get; set; }
        [JsonPropertyName("Tipus")]
        public string? uid { get; set; }
    }

    internal class Feljegyzesek
    {
        private static string Tartalom;
        private static string intezmenynev;
        private static int maString;
        
        public static string getFeljegyzesek(string access_token, string institute_code)
        {
            DateTime ma = DateTime.Now;
            ma.ToString("yyyy-MM-dd");
            return "fejlesztes alatt " + ma;
        }
    }
}
