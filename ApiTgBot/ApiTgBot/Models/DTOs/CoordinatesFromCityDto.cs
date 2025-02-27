using System.Text.Json.Serialization;

namespace ApiTgBot.Models.DTOs
{
    public class CoordinatesFromCityDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("local_names")]
        public object LocalName { get; set; }
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lng { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
