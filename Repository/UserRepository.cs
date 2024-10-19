using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

namespace MVCproyect.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlService _context;
        private readonly UserService _userService;
        public UserRepository(MySqlService context, UserService userService) 
        {
            _context = context;
            _userService = userService;
        }
        public async Task AddUserAsync(User newUser)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "INSERT INTO users (Username, PasswordHash, RoleId) VALUES (@name, @password, @role)";

                using MySqlCommand command = new MySqlCommand(query, connection);

                string hashedPassword = _userService.GeneratePasswordHash(newUser.PasswordHash);

                command.Parameters.AddWithValue("@name", newUser.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@role", newUser.RoleId);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> usersList = new List<User>();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM Users";

                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usersList.Add(new User
                        {
                            UserId = reader.GetInt32("UserId"),
                            UserName = reader.GetString("Username"),
                            PasswordHash = reader.GetString("PasswordHash"),
                            RoleId = reader.GetInt32("RoleId")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return usersList;
        }

        public Task UpdateUserAsync(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
