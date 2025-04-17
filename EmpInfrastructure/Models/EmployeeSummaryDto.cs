using System;
using System.Collections.Generic;
using System.Text;

namespace EmpInfrastructure.Models
{
    public class EmployeeSummaryDto
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }
        public string Email { get; set; }
    }
}
