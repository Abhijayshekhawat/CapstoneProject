using Microsoft.AspNetCore.Mvc;
//can be used for client editing profile
namespace CapstoneProject.Controllers.Client
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
