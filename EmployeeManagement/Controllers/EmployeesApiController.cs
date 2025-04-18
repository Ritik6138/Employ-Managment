using Employee.Repository.Models;
using Employee.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesApiController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Returns a list of employees in JSON or XML format depending on the Accept header.
        /// </summary>
        /// <returns>List of Employee objects.</returns>
        [HttpGet]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<IEnumerable<Employes>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
    }
}
