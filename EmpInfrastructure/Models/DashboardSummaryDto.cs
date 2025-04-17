using System;
using System.Collections.Generic;
using System.Text;

namespace EmpInfrastructure.Models
{
    public class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int RecentHires { get; set; }
        public int PendingTasks { get; set; }
        public List<EmployeeSummaryDto> RecentEmployees { get; set; }
    }
}
