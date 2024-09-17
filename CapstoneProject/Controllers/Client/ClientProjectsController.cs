using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers.Client
{
    public class ClientProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
