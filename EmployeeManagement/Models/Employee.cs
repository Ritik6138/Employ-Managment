using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        [Required]        
        public string Email { get; set; }       
        public string Password { get; set; }       
        public string ConfirmPassword { get; set; }
        [Required]
        public Dept? Department { get; set; } 
       
    }
}
