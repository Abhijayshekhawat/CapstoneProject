using Microsoft.AspNetCore.Mvc;
//should be used for admin to manage all projects
namespace CapstoneProject.Controllers.Admin
{
    public class AdminProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageProjects()
        {
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }
    }
}
