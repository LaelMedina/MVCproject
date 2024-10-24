using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task AddUserAsync(User newUser);

        Task UpdateUserAsync(User newUser);

        Task DeleteUserAsync(int id);
    }
}
