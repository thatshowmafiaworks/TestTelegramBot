namespace ApiTgBot.Models
{
    public class Weather
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string TimeZone { get; set; }
        public DateTime Date { get; set; }
        public string Units { get; set; }
        public string Weather_Overview { get; set; }

    }
}
