using Employee.Service.Interfaces;
using EmployeeManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly BreadCrumbService _breadCrumbService;
        public DashboardController(IEmployeeService employeeService,BreadCrumbService breadCrumbService)
        {
            _employeeService = employeeService;
            _breadCrumbService = breadCrumbService;
        }

        /// <summary>
        /// Displays the Dashboard for interactive reports.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns the dashboard view.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
            return View();
        }

        /// <summary>
        /// Retrieves employee headcount grouped by department.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHeadcountByDepartment()
        {
            try
            {
                var headcountData = await _employeeService.GetHeadcountByDepartmentAsync();
                return Json(headcountData);
            }
            catch (Exception ex)
            {
                // Optionally log exception ex
                return StatusCode(500, "An error occurred while retrieving headcount data.");
            }
        }

        /// <summary>
        /// Retrieves dashboard summary data.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSummaryData()
        {
            try
            {
                var summary = await _employeeService.GetDashboardSummaryAsync();
                return Json(summary);
            }
            catch (Exception ex)
            {
                // Optionally log exception ex
                return StatusCode(500, "An error occurred while retrieving summary data.");
            }
        }
    }
}
