using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly MockEmployeeRepository _empRepo;
        private static List<User> _users = new List<User>();
        public HomeController()
        {
            _empRepo = new MockEmployeeRepository();
        }

        public ViewResult Index()
        {
            var model = _empRepo.GetAllEmployee();
            
            return View(model);
        }

        public ViewResult Details(int Id)
        {
            var model = _empRepo.GetEmployee(Id);

            
            var _empVM = new EmployeeViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Department = model.Department
            };
            
            return View(_empVM);
        }

        [HttpGet]
        public ViewResult Edit(int?Id)
        {
            if (Id == null)
            {
                return View(new Employee());
            }
            else
            {
                //get employee details
                var _empModel = _empRepo.GetEmployee(1);
                return View(_empModel);
            }
        }

        [HttpPost]
        public IActionResult Edit(Employee employeeModel)
        {
            if (employeeModel.Id > 0 )
            {
                //update employee
                return View();
            }
            else
            {
                //add employee
                return View();
            }           

            //save changes

            //if changes saved return to index

            //if changes failed return employ edit view

        }

        public IActionResult Remove(int Id)
        {
            //Remove Record

            //Save changes

            //Redirect to index
            return RedirectToAction("Index");
        }

    }
}
