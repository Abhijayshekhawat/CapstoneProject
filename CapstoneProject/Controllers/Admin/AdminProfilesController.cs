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
            ViewBag.AdminViewProfiles = null; //null for now

            //Use stored procedure to get profile data from datatable
            //create profile objects and add them to the viewbag

            return View("~/Views/Admin/AdminManageProfiles.cshtml");
        }
    }
}
