using Microsoft.AspNetCore.Mvc;
//can be used for client dashboard functionalities
namespace CapstoneProject.Controllers.Client
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
