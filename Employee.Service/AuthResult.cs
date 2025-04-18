using Employee.Repository.Models;
using System.Collections.Generic;

namespace Employee.Service
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public User User { get; set; }
        public List<string> Errors { get; } = new List<string>();

        // Success factory method
        public static AuthResult Success(User user = null)
            => new AuthResult { Succeeded = true, User = user };

        // Failure factory method
        public static AuthResult Failure(params string[] errors)
        {
            var result = new AuthResult { Succeeded = false };
            result.Errors.AddRange(errors); 
            return result;
        }
    }

}
