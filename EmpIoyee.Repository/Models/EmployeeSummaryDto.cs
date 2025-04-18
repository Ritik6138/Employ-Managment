using System;
using System.Collections.Generic;
using System.Text;

namespace Employee.Repository.Models
{
    public class EmployeeSummaryDto
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }
        public string Email { get; set; }
    }
}
