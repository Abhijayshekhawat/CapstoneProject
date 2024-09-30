using Microsoft.AspNetCore.Mvc;
//should be used for clients to edit their profile
namespace CapstoneProject.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClientProfile()
        {
            return View();
        }

        public IActionResult ClientProfileEdit()
        {
            return View();
        }

        public IActionResult ClientProjects()
        {
            return View();
        }

    }
}
