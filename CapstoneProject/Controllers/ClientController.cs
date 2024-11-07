using CapstoneProject.Attributes;
using CapstoneProject.Models.ClassLibrary;
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

        public IActionResult ClientProjectEdit() {
        
            
        
            return View("~/Views/Client/ClientProjectEdit.cshtml");
        
        }

        public IActionResult ClientProfileEdit()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");
            return View();
        }

        public IActionResult EditClientProfile()
        {
            // Manually extract from form
            Profile profile = new Profile
            {
                FirstName = Request.Form["FirstName"].ToString(),
                LastName = Request.Form["LastName"].ToString(),
                Email = Request.Form["Email"].ToString(),  // Extract email from form   
                Organization = Request.Form["Organization"].ToString(),  // Extract organization from form
                SubmissionDate = DateTime.Now,  // will use this variable as Edit Date
                Status = "Approved", //need to double check what status means approved
                ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID")) //need to double check this
            //need to look over status, submission date (which will be edited date), and user type
           //then will call EditProfileMethod and check whether rows were affected 
            };
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
        [AuthorizeRoles("Client")]
        public IActionResult ClientDashboard()
        {
            NewProjects newProjects = new NewProjects();
            List<NewProjects> NewProjectList = newProjects.GetNewProjects();
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            return View("~/Views/Client/ClientDashboard.cshtml", NewProjectList);
        }

    }
}
