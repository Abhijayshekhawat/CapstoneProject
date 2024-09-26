using Microsoft.AspNetCore.Mvc;
//should be used for admin to manage all user profiles
namespace CapstoneProject.Controllers.Admin
{
    public class AdminProfilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageProfiles()
        {
            return View("~/Views/Admin/AdminManageProfiles.cshtml");
        }
    }
}
