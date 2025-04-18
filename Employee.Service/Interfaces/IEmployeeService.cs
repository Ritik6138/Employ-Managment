using Employee.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employes>> GetAllEmployeesAsync();
        Task<Employes> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employes employee);
        Task<bool> UpdateEmployeeAsync(Employes employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<HeadcountData> GetHeadcountByDepartmentAsync();
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    }

}
