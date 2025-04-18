using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Employee.Repository.Models
{
    public class Employes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Dept Department { get; set; }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
    }

    public enum Dept
    {
        HR,
        IT,
        Finance,
        Sales
    }
}
