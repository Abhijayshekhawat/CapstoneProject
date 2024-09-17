using Microsoft.AspNetCore.Mvc;
//should be used for clients to edit their profile
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
