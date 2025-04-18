using Employee.Repository.Models;
using System.Threading.Tasks;

namespace Employee.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(User user, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
