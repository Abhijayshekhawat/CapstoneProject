using Microsoft.AspNetCore.Mvc;
//can be used for client projects such as submitted projects
namespace CapstoneProject.Controllers.Client
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
