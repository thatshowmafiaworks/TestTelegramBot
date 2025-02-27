using ApiTgBot.Models;
using ApiTgBot.Models.DTOs;

namespace ApiTgBot.Services
{
    public interface IWeatherService
    {
        Task<string> GetForecast(CoordinatesDto coordinates);
        Task<string> GetForecast(City city);
    }
}