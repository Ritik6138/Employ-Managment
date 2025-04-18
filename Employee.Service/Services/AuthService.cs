using Employee.Repository.Interfaces;
using Employee.Repository.Models;
using Employee.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PasswordVerificationResultCore = Microsoft.AspNetCore.Identity.PasswordVerificationResult;
namespace Employee.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IAuthRepository authRepo,
            IPasswordHasher<User> passwordHasher)
        {
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResult> RegisterAsync(User user, string password)
        {
            if (await _authRepo.EmailExistsAsync(user.Email))
                return AuthResult.Failure("Email already registered");

            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _authRepo.AddUserAsync(user);
            return AuthResult.Success();
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _authRepo.GetUserByEmailAsync(email);
            if (user == null) return AuthResult.Failure("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResultCore.Success)
            {
                return AuthResult.Success(user);
            }
            else
            {
                return AuthResult.Failure("Invalid credentials");
            }
        }


        public Task LogoutAsync() => Task.CompletedTask;
    }
}
