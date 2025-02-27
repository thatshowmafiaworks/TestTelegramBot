using ApiTgBot.Models.DTOs;

namespace ApiTgBot.Services
{
    public interface IWeatherService
    {
        Task<string> GetForecast(CoordinatesDto coordinates);
    }
}