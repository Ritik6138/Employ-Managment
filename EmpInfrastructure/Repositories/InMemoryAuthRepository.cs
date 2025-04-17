using EmpInfrastructure.Interfaces;
using EmpInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmpInfrastructure.Repositories
{
    public class InMemoryAuthRepository : IAuthRepository
    {
        private readonly List<User> _users = new List<User>(); 
        private int _nextId = 1;

        public Task AddUserAsync(User user)
        {
            user.Id = Interlocked.Increment(ref _nextId); // Thread-safe ID increment
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return Task.FromResult(_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<bool> UsernameExistsAsync(string username)
        {
            return Task.FromResult(_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
