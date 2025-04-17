using EmpInfrastructure.Interfaces;
using EmpInfrastructure.Models;
using EmployeeCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() =>
            await _repository.GetAllAsync();

        public async Task<Employee> GetEmployeeByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task CreateEmployeeAsync(Employee employee)
        {
            var validationContext = new ValidationContext(employee);
            Validator.ValidateObject(employee, validationContext, validateAllProperties: true);

            employee.HireDate = DateTime.UtcNow; // Enforce server-set HireDate
            await _repository.AddAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                await _repository.UpdateAsync(employee);
                return true;
            }
            catch (Exception)
            { 
                return false;
            }
        }
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existingEmployee = await _repository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return false;
            }
            await _repository.DeleteAsync(id);
            return true;
        }
        public async Task<HeadcountData> GetHeadcountByDepartmentAsync()
        {
            var employees = await _repository.GetAllAsync();
            var allDepartments = Enum.GetNames(typeof(Dept)).ToList();
            var groupedData = employees
                .GroupBy(e => e.Department)
                .Select(g => new { Department = g.Key.ToString(), Count = g.Count() })
                .ToList();
            var labels = new List<string>();
            var counts = new List<int>();

            foreach (var dept in allDepartments)
            {
                labels.Add(dept);

                var group = groupedData.FirstOrDefault(x => x.Department == dept);
                counts.Add(group != null ? group.Count : 0);
            }
            var headcountData = new HeadcountData
            {
                Labels = labels,
                Counts = counts
            };

            return headcountData;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var employees = await _repository.GetAllAsync();
            var department = await _repository.GetAllDepartmentsAsync();
            int totalEmployees = employees.Count();
            int totalDepartments = department.Count();
            DateTime recentThreshold = DateTime.Now.AddDays(-30);
            int recentHiresCount = employees.Count(e => e.HireDate >= recentThreshold);
            int pendingTasks = 0;
            var recentEmployees = employees
                .OrderByDescending(e => e.HireDate)
                .Take(5)
                .Select(e => new EmployeeSummaryDto
                {
                    Name = e.Name,
                    Department = e.Department.ToString(),
                    HireDate = e.HireDate,
                    Email = e.Email
                })
                .ToList();

            return new DashboardSummaryDto
            {
                TotalEmployees = totalEmployees,
                TotalDepartments = totalDepartments,
                RecentHires = recentHiresCount,
                PendingTasks = pendingTasks,
                RecentEmployees = recentEmployees
            };
        }
    }
}
