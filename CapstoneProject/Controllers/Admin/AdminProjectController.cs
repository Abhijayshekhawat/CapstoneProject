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
            ViewBag.AdminViewProjects = null; //null for now

            //Use stored procedure to get project data from datatable
            //create project objects and add them to the viewbag

            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewProjects()
        {
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }
    }
}
