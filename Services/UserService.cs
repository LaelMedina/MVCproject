using System.Data;
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

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        User user = await GetUserByNameAsync(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return true;
        }
        return false;
    }
}
