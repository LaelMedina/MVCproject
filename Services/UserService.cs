using System.Data;
using MVCproyect.Models;
using MySql.Data.MySqlClient;

public class UserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User GetUserByUsername(string username)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Users WHERE Username = @username";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        UserId = reader.GetInt32("Id"),
                        UserName = reader.GetString("Username"),
                        PasswordHash = reader.GetString("PasswordHash"),
                        RolId = reader.GetInt32("Role")
                    };
                }
            }
        }
        return null;
    }

    public bool ValidateUser(string username, string password)
    {
        User user = GetUserByUsername(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return true;
        }
        return false;
    }
}
