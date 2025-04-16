using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    public class UserViewModel
    {
        public User NewUser { get; set; }
        public List<User> Users { get; set; }
    }
}
