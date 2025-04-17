using EmpInfrastructure.Models;
using System.Threading.Tasks;

namespace EmployeeCore.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(User user, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
