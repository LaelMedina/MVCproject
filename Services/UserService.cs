using System.Data;
using MVCproyect.Models;
using MySql.Data.MySqlClient;

public class UserService
{
    private readonly MySqlConnection _connection;

    public UserService(MySqlService connection)
    {
        _connection = connection.CreateConnection();
    }

    public User GetUserByUsername(string username)
    {
        try
        {
            string query = "SELECT * FROM Users WHERE Username = @username";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@username", username);

            _connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        UserId = reader.GetInt32("Id"),
                        UserName = reader.GetString("Username"),
                        PasswordHash = reader.GetString("PasswordHash"),
                        RoleId = reader.GetInt32("Role")
                    };
                }
            }
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(ex.Message);
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
