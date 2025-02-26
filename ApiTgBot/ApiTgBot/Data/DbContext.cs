using ApiTgBot.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Z.Dapper.Plus;

namespace ApiTgBot.Data
{
    public class DbContext: IDbContext
    {
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
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
            catch(Exception ex)
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

        public async Task UpdateUser(User user)
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
    }
}
