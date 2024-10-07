using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    public class UserDashController : Controller
    {
        // This method returns the UserDashboard view
        public IActionResult UserDashboard()
        {
            // Specify the path to the view inside the "Dashboard" folder
            return View("~/Views/Dashboard/UserDashboard.cshtml");
        }
    }
}



//app.MapControllerRoute(
  //  name: "default",
    //pattern: "{controller=UserDash}/{action=UserDashboard}/{id?}");