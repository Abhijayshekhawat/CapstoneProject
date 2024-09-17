using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers.Admin
{
    public class AdminProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
