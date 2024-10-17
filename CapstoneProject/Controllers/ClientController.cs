using Microsoft.AspNetCore.Mvc;
using System.Data;
//should be used for clients to edit their profile
namespace CapstoneProject.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClientProfile()
        {
            return View();
        }

        public IActionResult ClientProfileEdit()
        {
            return View();
        }

        public IActionResult ClientProjects()
        {
            //Use stored procedure to get project data from datatable
            //create project objects and add them to the viewbag
            DataSet ds = new DataSet();
                
            //will need to use user.GetProjectsById in order to pass list of projects by current user

            return View();
        }

    }
}
