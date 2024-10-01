using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByNameAsync(string username);

        Task<bool> ValidateUserAsync(string username, string password);
    }
}
