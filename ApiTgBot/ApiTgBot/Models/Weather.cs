using System.Text.Json.Serialization;

namespace ApiTgBot.Models
{
    public class Weather
    {
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lng { get; set; }
        [JsonPropertyName("tz")]
        public string TimeZone { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("units")]
        public string Units { get; set; }
        [JsonPropertyName("weather_overview")]
        public string WeatherOverview { get; set; }

    }
}
