using EmployeeManagement.Models;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Dept? Department { get; set; }
    }
}
