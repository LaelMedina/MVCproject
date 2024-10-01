using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByName(string username);

        Task<bool> ValidateUser(string username, string password);
    }
}
