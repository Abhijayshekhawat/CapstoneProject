using Microsoft.AspNetCore.Mvc;
//dashboard controller for admin/reviewer
namespace CapstoneProject.Controllers.Admin
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
