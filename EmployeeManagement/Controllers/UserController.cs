using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    /// <summary>
    /// Controller to handle CRUD operations for users.
    /// Created by Ritik Kumar.
    /// </summary>
    public class UserController : Controller
    {
        private readonly MockUserRepository _repo = MockUserRepository.Instance;

        /// <summary>
        /// Displays the main index page.
        /// </summary>
        public IActionResult Index()
        {
            var model = new UserViewModel
            {
                Users = _repo.GetAllUsers().ToList()
            };
            return View(model);
        }

        /// <summary>
        /// GET: Renders a form to add a new user.
        /// </summary>
        [HttpGet]
        public IActionResult UserForm()
        {
            return View();
        }

        /// <summary>
        /// POST: Handles form submission to add a new user.
        /// Validates input and adds the user to the repository.
        /// </summary>
        /// <param name="user">User object from the form.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserForm(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Add(user); // Add user to the repository
                    ViewData["SuccessMessage"] = "User added successfully!";
                    var model = new UserViewModel
                    {
                        Users = _repo.GetAllUsers().ToList()
                    };
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return StatusCode(500); // Internal server error
                }
            }

            // Return validation errors if the model state is invalid
            return View("Index", new UserViewModel
            {
                Users = _repo.GetAllUsers().ToList(),
                NewUser = user
            });
        }

        /// <summary>
        /// Returns a partial view for user-specific components.
        /// </summary>
        public IActionResult GetUserPartial()
        {
            return PartialView("_UserPartial", new User());
        }

        /// <summary>
        /// Displays a list of all users.
        /// </summary>
        public IActionResult UserList()
        {
            return View(_repo.GetAllUsers()); // Fetch all users
        }

        /// <summary>
        /// GET: Renders a view to edit an existing user.
        /// </summary>
        /// <param name="id">User ID.</param>
        public IActionResult Edit(int id)
        {
            var user = _repo.GetUser(id); // Fetch user by ID
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            return View(user);
        }

        /// <summary>
        /// POST: Handles updates to an existing user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="user">Updated user data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound(); // Return 404 if IDs don't match
            }

            if (ModelState.IsValid)
            {
                _repo.Update(user); // Update user in repository
                return RedirectToAction(nameof(Index)); // Redirect to index
            }

            return View(user); // Return view with validation errors
        }

        /// <summary>
        /// GET: Renders a confirmation page to delete a user.
        /// </summary>
        /// <param name="id">User ID.</param>
        public IActionResult Delete(int id)
        {
            var user = _repo.GetUser(id); // Fetch user by ID
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            return View(user);
        }

        /// <summary>
        /// POST: Confirms and deletes a user.
        /// </summary>
        /// <param name="id">User ID.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Remove(id); // Remove user from repository
            return RedirectToAction(nameof(Index)); // Redirect to index
        }
    }
}
