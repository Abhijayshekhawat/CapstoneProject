using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers.Client
{
    public class ClientProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
