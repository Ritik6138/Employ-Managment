using EmpInfrastructure.Models;
using EmployeeCore.Interfaces;
using EmployeeManagement.Models;
using EmployeeManagement.Service;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dept = EmpInfrastructure.Models.Dept;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly BreadCrumbService _breadCrumbService;
        /// <summary>
        /// Constructor to initialize the employee service.
        /// </summary>
        /// Created by: Ritik Kumar
        public EmployeeController(IEmployeeService employeeService, BreadCrumbService service)
        {
            _employeeService = employeeService;
            _breadCrumbService = service;
        }

        /// <summary>
        /// This method displays the list of employees with search, sort, and pagination functionality.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="searchString">The search string to filter employees by name.</param>
        /// <param name="sortOrder">The sort order (e.g., name or date).</param>
        /// <param name="pageNumber">The current page number for pagination.</param>
        /// <returns>Returns the employee list view.</returns>
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            try
            {
                ViewData["CurrentFilter"] = searchString;
                ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
                ViewData["DateSortParam"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
                var employees = await _employeeService.GetAllEmployeesAsync();

                var viewModel = employees.Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Department = (Models.Dept?)e.Department,
                    HireDate = e.HireDate
                });

                if (!string.IsNullOrEmpty(searchString))
                {
                    viewModel = viewModel.Where(e => e.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                }

                viewModel = sortOrder switch
                {
                    "name_desc" => viewModel.OrderByDescending(e => e.Name),
                    "date_desc" => viewModel.OrderByDescending(e => e.HireDate),
                    _ => viewModel.OrderBy(e => e.Name)
                };

                int pageSize = 10;
                var paginatedResult = PaginatedList<EmployeeViewModel>.Create(
                    viewModel.AsQueryable(),
                    pageNumber ?? 1,
                    pageSize
                );

                return View(paginatedResult);
            }
            catch (Exception ex)
            {
                // Log exception (consider using ILogger for real-world scenarios)
                TempData["ErrorMessage"] = "An unexpected error occurred while fetching the employee list.";
                return RedirectToAction("Error", "Home"); // Redirect to a global error page
            }
        }

        /// <summary>
        /// This method displays the create employee page.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns the create employee view.</returns>
        public IActionResult Create()
        {
            try
            {
                ViewBag.Departments = Enum.GetValues(typeof(Dept))
                    .Cast<Dept>()
                    .Select(d => new SelectListItem(d.ToString(), d.ToString()));
                ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
                return View();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while loading the create employee page.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method handles the creation of a new employee.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="viewModel">The employee view model containing input data.</param>
        /// <returns>Redirects to the Index view if successful, otherwise returns the create view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new Employee
                    {
                        Name = viewModel.Name,
                        Email = viewModel.Email,
                        Department = (Dept)viewModel.Department.Value,
                        HireDate = viewModel.HireDate
                    };

                    await _employeeService.CreateEmployeeAsync(employee);
                    TempData["SuccessMessage"] = "Employee created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Departments = Enum.GetValues(typeof(Dept))
                    .Cast<Dept>()
                    .Select(d => new SelectListItem(d.ToString(), d.ToString()));

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the employee.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method displays details of a specific employee.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="id">The ID of the employee to display details for.</param>
        /// <returns>Returns the details view of the specified employee.</returns>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new EmployeeViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = (Models.Dept?)employee.Department,
                    HireDate = employee.HireDate
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while fetching employee details.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method displays the edit employee page.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="id">The ID of the employee to edit.</param>
        /// <returns>Returns the edit employee view.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new EmployeeViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = (Models.Dept?)employee.Department,
                    HireDate = employee.HireDate
                };

                ViewBag.Departments = Enum.GetValues(typeof(Dept))
                    .Cast<Dept>()
                    .Select(d => new SelectListItem(d.ToString(), d.ToString()));

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while loading the edit employee page.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method handles the update of an employee's information.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="viewModel">The employee view model containing updated data.</param>
        /// <returns>Redirects to the Index view if successful, otherwise returns the edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    TempData["ErrorMessage"] = "Invalid employee ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    var employee = new Employee
                    {
                        Id = viewModel.Id,
                        Name = viewModel.Name,
                        Email = viewModel.Email,
                        Department = (Dept)viewModel.Department.Value,
                        HireDate = viewModel.HireDate
                    };

                    var result = await _employeeService.UpdateEmployeeAsync(employee);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Employee updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update employee. Please try again.";
                    }
                }

                ViewBag.Departments = Enum.GetValues(typeof(Dept))
                    .Cast<Dept>()
                    .Select(d => new SelectListItem(d.ToString(), d.ToString()));

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while updating the employee.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method displays the delete confirmation page for a specific employee.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>Returns the delete confirmation view.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new EmployeeViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = (Models.Dept?)employee.Department,
                    HireDate = employee.HireDate
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while loading the delete confirmation page.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method confirms and deletes the specified employee.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _employeeService.DeleteEmployeeAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Employee deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete employee.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the employee.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// This method provides a JSON API endpoint to retrieve all employees.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns a JSON object containing all employees.</returns>
        [HttpGet("api/employees")]
        public async Task<IActionResult> GetEmployeesJson()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving employees." });
            }
        }

        /// <summary>
        /// This method provides an XML SOAP API endpoint to retrieve all employees.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns an XML SOAP object containing all employees.</returns>
        [HttpGet("api/employees/soap")]
        [Produces("application/xml")]
        public async Task<IActionResult> GetEmployeesXml()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(new SoapEnvelope<Employee>
                {
                    Body = new SoapBody<Employee>
                    {
                        Items = employees.ToList()
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving employees in XML format." });
            }
        }
    }

    // XML DTO Classes
    [XmlRoot("Envelope")]
    public class SoapEnvelope<T>
    {
        [XmlElement("Body")]
        public SoapBody<T> Body { get; set; }
    }

    public class SoapBody<T>
    {
        [XmlElement("Item")]
        public List<T> Items { get; set; }
    }
}