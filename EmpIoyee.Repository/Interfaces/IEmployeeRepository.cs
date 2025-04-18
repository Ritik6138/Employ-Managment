using Employee.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employes>> GetAllAsync();
        Task<Employes> GetByIdAsync(int id);
        Task AddAsync(Employes employee);
        Task UpdateAsync(Employes employee);
        Task DeleteAsync(int id);
        Task<IEnumerable<Dept>> GetAllDepartmentsAsync();
    }
}
