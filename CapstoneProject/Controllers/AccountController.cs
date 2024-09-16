using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
    }
}
