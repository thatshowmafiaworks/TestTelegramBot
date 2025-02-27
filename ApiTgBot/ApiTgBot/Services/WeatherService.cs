using ApiTgBot.Data;
using ApiTgBot.Models.DTOs;
using static System.Net.WebRequestMethods;

namespace ApiTgBot.Services
{
    public class WeatherService(IConfiguration config, IDbContext context): IWeatherService
    {
        public async Task<string> GetForecast(CoordinatesDto coordinates)
        {
            var apiKey = config["OpenWeatherToken"];
            var url = $"https://api.openweathermap.org/data/3.0/onecall/overview?lat={coordinates.Lat}&lon={coordinates.Lng}&appid={apiKey}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return "Aboba";
        }
    }
}
