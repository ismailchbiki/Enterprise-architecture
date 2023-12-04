using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepo
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
    }
}