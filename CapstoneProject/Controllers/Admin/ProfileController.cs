using Microsoft.AspNetCore.Mvc;
//use for admin managing profiles
namespace CapstoneProject.Controllers.Admin
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
