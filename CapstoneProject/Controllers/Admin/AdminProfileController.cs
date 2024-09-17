using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers.Admin
{
    public class AdminProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
