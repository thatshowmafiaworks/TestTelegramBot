﻿using ApiTgBot.Models;
using ApiTgBot.Models.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Z.Dapper.Plus;

namespace ApiTgBot.Data
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;
        private readonly ILogger<DbContext> _logger;

        public DbContext(IConfiguration configuration, ILogger<DbContext> logger)
        {
            _connectionString = configuration["ConnectionString"];
            _logger = logger;
        }

        public async Task<IEnumerable<Models.User>> GetAllUsers()
        {
            var query = "SELECT * from Users";

            using var connection = new SqlConnection(_connectionString);
            var users = (await connection.QueryAsync<Models.User>(query)).ToList();
            return users;
        }
        public async Task<Models.User> GetUser(string username)
        {
            var query = "SELECT * from Users Where Username = @username";
            using var connection = new SqlConnection(_connectionString);
            var user = (await connection.QuerySingleOrDefaultAsync<Models.User>(query, new { username = username }));
            return user;
        }
        public async Task<Models.User> GetUser(long id)
        {
            var query = "SELECT * from Users Where Id = @id";
            using var connection = new SqlConnection(_connectionString);
            Models.User user = default;
            try
            {
                user = (await connection.QuerySingleOrDefaultAsync<Models.User>(query, new { id = id }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception!!!:::{ex.Message}");
            }
            return user;
        }

        public async Task AddUser(Models.User user)
        {
            var existing = await GetUser(user.Id);
            if (existing is null)
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.SingleInsertAsync(user);
            }
        }

        public async Task UpdateUser(Models.User user)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.SingleUpdateAsync(user);
        }

        public async Task AddCity(string name)
        {
            var query = "INSERT INTO Cities (Name) Values(@name)";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { name = name });
        }

        public async Task<City> GetCity(long id)
        {
            var query = "SELECT * FROM Cities where Id = @id";
            using var connection = new SqlConnection(_connectionString);
            var city = await connection.QuerySingleOrDefaultAsync<City>(query, new { id = id });
            return city;
        }

        public async Task<City> GetCity(string name)
        {
            var query = "SELECT * from Cities Where Name = @name";
            using var connection = new SqlConnection(_connectionString);
            var city = await connection.QuerySingleOrDefaultAsync<City>(query, new { name = name });
            return city;
        }

        public async Task SetCoordinates(long userId, CoordinatesDto coordinates)
        {
            var user = await GetUser(userId);
            user.Lat = coordinates.Lat;
            user.Lng = coordinates.Lng;
            await UpdateUser(user);
            _logger.LogInformation($"Added coordinates to user:{user.Username}\t with coordinates:lat'{user.Lat}' lng:'{user.Lng}'");
        }

        public async Task<CoordinatesDto> GetCoordinates(long userId)
        {
            var user = await GetUser(userId);
            var coordinates = new CoordinatesDto { Lat = user.Lat, Lng = user.Lng };
            return coordinates;
        }

        public async Task<UserHistory> GetUserHistory(long userId)
        {
            var query = "select * from UserHistories where UserId = @userId";
            using var connection = new SqlConnection(_connectionString);
            var history = await connection.QueryFirstOrDefaultAsync<UserHistory>(query, new { userId = userId });
            if (history is null)
            {
                await CreateUserHistory(userId);
                return await GetUserHistory(userId);
            }
            return history;
        }

        public async Task CreateUserHistory(long userId)
        {
            var history = new UserHistory { UserId = userId };
            using var connection = new SqlConnection(_connectionString);
            await connection.SingleInsertAsync(history);

        }

        public async Task AddHistoryRecord(HistoryRecord record)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.SingleInsertAsync(record);
        }

        public async Task<IEnumerable<HistoryRecord>> GetHistoryRecordsForUser(long userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var userHistory = await GetUserHistory(userId);

            var query = $"Select * from HistoryRecords where HistoryId = {userHistory.Id}";


            var records = await connection.QueryAsync<HistoryRecord>(query);
            return records;
        }
    }
}
