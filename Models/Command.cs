using System.Text.Json.Serialization;

namespace testirovanie.Models
{
    public class Command
    {
        [JsonPropertyName("id")]
        public int command_id { get; set; }
        public string name { get; set; } = string.Empty;
        [JsonPropertyName("parameter_name1")]
        public string? parametr1_name { get; set; }
        [JsonPropertyName("parameter_name2")]
        public string? parametr2_name { get; set; }
        [JsonPropertyName("parameter_name3")]
        public string? parametr3_name { get; set; }
        [JsonPropertyName("parameter_default_value1")]
        public int? parametr1_default { get; set; }
        [JsonPropertyName("parameter_default_value2")]
        public int? parametr2_default { get; set; }
        [JsonPropertyName("parameter_default_value3")]
        public int? parametr3_default { get; set; }
    }
}
