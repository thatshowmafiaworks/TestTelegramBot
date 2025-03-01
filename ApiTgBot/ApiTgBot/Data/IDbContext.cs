using ApiTgBot.Models;
using ApiTgBot.Models.DTOs;

namespace ApiTgBot.Data
{
    public interface IDbContext
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string username);
        Task<User> GetUser(long id);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task AddCity(string name);
        Task<City> GetCity(long id);
        Task<City> GetCity(string name);
        Task SetCoordinates(long userId, CoordinatesDto coordinates);
        Task<CoordinatesDto> GetCoordinates(long userId);
        Task CreateUserHistory(long userId);
        Task<UserHistory> GetUserHistory(long userId);
        Task AddHistoryRecord(HistoryRecord record);
        Task<IEnumerable<HistoryRecord>> GetHistoryRecordsForUser(long userId);
    }
}