using System.Data;
using System.Security.Cryptography;
using System.Text;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

public class UserService : IUserService
{
    private readonly MySqlService _context;

    public UserService(MySqlService context)
    {
        _context = context;
    }

    public async Task<User> GetUserByNameAsync(string username)
    {

        User user = new User();

        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "SELECT * FROM Users WHERE Username = @username";

            MySqlCommand command = new MySqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@username", username);

            using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return user = new User
                    {
                        UserId = reader.GetInt32("UserId"),
                        UserName = reader.GetString("Username"),
                        PasswordHash = reader.GetString("PasswordHash"),
                        RoleId = reader.GetInt32("RoleId")
                    };
                }
            }

        }
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        }

        return user;
    }

    public async Task<List<Role>> GetUsersRolesAsync()
    {
        List<Role> _rolesList = new List<Role>();

        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "SELECT RoleId, RoleName FROM roles";

            using MySqlCommand command = new MySqlCommand(query, connection);

            using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Role role = new Role()
                {
                    RoleId = reader.GetInt32("RoleId"),
                    RoleName = reader.GetString("RoleName"),
                };

                _rolesList.Add(role);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return _rolesList;
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        User user = await GetUserByNameAsync(username);
        return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public string GeneratePasswordHash(string password) 
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

}
