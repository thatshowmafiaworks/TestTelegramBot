using ApiTgBot.Data;
using ApiTgBot.Models;
using ApiTgBot.Models.DTOs;
using System.Globalization;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace ApiTgBot.Services
{
    public class WeatherService(IConfiguration config, IDbContext context): IWeatherService
    {
        public async Task<string> GetForecast(CoordinatesDto coordinates)
        {
            var apiKey = config["OpenWeatherToken"];
            var lat = coordinates.Lat.ToString(CultureInfo.InvariantCulture);
            var lng = coordinates.Lng.ToString(CultureInfo.InvariantCulture);
            var url = $"https://api.openweathermap.org/data/3.0/onecall/overview?lat={lat}&lon={lng}&appid={apiKey}&units=metric";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            Weather weather = JsonSerializer.Deserialize<Weather>(responseBody);
            //Console.WriteLine(weather.WeatherOverview);
            return weather.WeatherOverview;
        }

        public async Task<string> GetForecast(City city)
        {
            var apikey = config["OpenWeatherToken"];

            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city.Name}&appid={apikey}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            IEnumerable<CoordinatesFromCityDto> coord = JsonSerializer.Deserialize<IEnumerable<CoordinatesFromCityDto>>(responseBody);
            var coordinates = new CoordinatesDto { Lat = coord.First().Lat, Lng = coord.First().Lng };

            return await GetForecast(coordinates);
        }
    }
}
