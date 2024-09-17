using Microsoft.AspNetCore.Mvc;
//admin/reviewer controls for projects
namespace CapstoneProject.Controllers.Admin
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
