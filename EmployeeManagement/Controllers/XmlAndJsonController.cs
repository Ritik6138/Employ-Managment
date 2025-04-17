using EmployeeManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class XmlAndJsonController : Controller
    {
        private readonly BreadCrumbService _breadCrumbService;
        public XmlAndJsonController(BreadCrumbService breadCrumbService)
        {
            _breadCrumbService = breadCrumbService;
        }

        public IActionResult Index()
        {
            ViewBag.Breadcrumbs = _breadCrumbService.GetBreadCumbs();
            return View();
        }
    }
}
