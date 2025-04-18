using Employee.Repository.Interfaces;
using Employee.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Repository.Repositories
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employes> _employees = new List<Employes>()
        {
            new Employes { Id = 1, Name = "Mark", Department = Dept.IT, Email = "Mark@hotmil.com" },
            new Employes { Id = 2, Name = "Param", Department = Dept.IT, Email = "param@hotmil.com" },
            new Employes { Id = 3, Name = "Tom", Department = Dept.HR, Email = "Tom@hotmil.com" }
        };
        private int _nextId = 4; // Start with the next ID after the predefined data

        public Task AddAsync(Employes employee)
        {
            employee.Id = Interlocked.Increment(ref _nextId); // Generate a unique ID
            _employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Employes>> GetAllAsync() =>
            Task.FromResult(_employees.AsEnumerable());

        public Task<Employes> GetByIdAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }

        public Task UpdateAsync(Employes employee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existingEmployee == null)
                throw new KeyNotFoundException($"Employee with ID {employee.Id} not found.");

            // Update properties including HireDate if needed
            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Department = employee.Department;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var employeeToRemove = _employees.FirstOrDefault(e => e.Id == id);
            if (employeeToRemove != null)
            {
                _employees.Remove(employeeToRemove);
            }
            return Task.CompletedTask;
        }
        public Task<IEnumerable<Dept>> GetAllDepartmentsAsync()
        {
            var departments = _employees
                .Select(e => e.Department)
                .Distinct();
            return Task.FromResult(departments);
        }
    }
}
