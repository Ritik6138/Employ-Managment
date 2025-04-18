using Employee.Repository.Models;
using System.Threading.Tasks;

namespace Employee.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task AddUserAsync(User user);
    }
}
