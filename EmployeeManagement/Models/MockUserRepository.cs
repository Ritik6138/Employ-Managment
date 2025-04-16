using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    /// <summary>
    /// Singleton repository to manage user data.
    /// Created by Ritik Kumar.
    /// </summary>
    public class MockUserRepository
    {
        // Lazy initialization for singleton instance
        private static readonly Lazy<MockUserRepository> _instance = new Lazy<MockUserRepository>(() => new MockUserRepository());

        /// <summary>
        /// Gets the singleton instance of the repository.
        /// </summary>
        public static MockUserRepository Instance => _instance.Value;

        // Internal list to store user data
        private readonly List<User> _userList;

        // Tracks the next ID for new users
        private int _nextId = 4;

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private MockUserRepository()
        {
            _userList = new List<User>
            {
                new User { Id = 1, Name = "Ritik", Email = "ritik@example.com", Password = "password123" },
                new User { Id = 2, Name = "Anuj", Email = "anuj@example.com", Password = "securepass456" },
                new User { Id = 3, Name = "Bipin", Email = "bipin@example.com", Password = "bipin789" }
            };
        }

        /// <summary>
        /// Retrieves all users from the repository.
        /// </summary>
        /// <returns>Enumerable of users.</returns>
        public IEnumerable<User> GetAllUsers() => _userList;

        /// <summary>
        /// Retrieves a single user by their ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User object if found, otherwise null.</returns>
        public User GetUser(int id) => _userList.FirstOrDefault(u => u.Id == id);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">User object to add.</param>
        /// <returns>The added user with their new ID.</returns>
        public User Add(User user)
        {
            user.Id = _nextId++; // Assign a unique ID
            _userList.Add(user);
            return user;
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="userUpdate">User object containing updated details.</param>
        /// <returns>The updated user object if successful, otherwise null.</returns>
        public User Update(User userUpdate)
        {
            var user = GetUser(userUpdate.Id); // Find the user by ID
            if (user != null)
            {
                // Update properties
                user.Name = userUpdate.Name;
                user.Email = userUpdate.Email;
                user.Password = userUpdate.Password;
            }
            return user;
        }

        /// <summary>
        /// Removes a user from the repository by their ID.
        /// </summary>
        /// <param name="id">User ID to remove.</param>
        /// <returns>The removed user object if found, otherwise null.</returns>
        public User Remove(int id)
        {
            var userToRemove = GetUser(id); // Find user by ID
            if (userToRemove != null)
            {
                _userList.Remove(userToRemove); // Remove from the list
            }
            return userToRemove;
        }
    }
}
