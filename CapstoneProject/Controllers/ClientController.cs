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
        public IActionResult ReadOnlyProject(int ProjectID)
        {

            NewProjects newProjects = new NewProjects();
            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            newProjects = newProjects.GetNewProjectByProjectID(ProjectID, newProjects.ProfileID);


            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            return View("~/Views/Client/ReadOnlyProject.cshtml", newProjects);
        }
        public IActionResult ClientProfile()
        {
            return View();
        }

        public IActionResult EditClientProfile() 
        {

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");
            return View();
            
        
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

        public IActionResult EditClientProject(int ProjectID)
        {

            NewProjects newProjects = new NewProjects();
            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            newProjects = newProjects.GetNewProjectByProjectID(ProjectID,newProjects.ProfileID);
           

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            return View("~/Views/Client/EditClientProject.cshtml", newProjects);


           


        }

        public IActionResult UpdateClientProject(int ProjectID)
        {
            NewProjects newProjects = new NewProjects();
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();

            newProjects.UpdateClientProject(ProjectID,newProjects.ProjectDescription, newProjects.ProjectName);


            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            List<NewProjects> NewProjectList = newProjects.GetNewProjects(newProjects.ProfileID);
            ViewBag.ProfileStatus = status.GetProfileStatus(newProjects.ProfileID);

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            return View("~/Views/Client/ClientDashboard.cshtml", NewProjectList);
        }


        public IActionResult UpdateClientProfile()
        {
            // Manually extract from form
            Profile profile = new Profile
            {
                FirstName = Request.Form["FirstName"].ToString(),
                LastName = Request.Form["LastName"].ToString(),
                Email = Request.Form["Email"].ToString(),  // Extract email from form   
                Organization = Request.Form["Organization"].ToString(),  // Extract organization from form
                ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID")) //need to double check this
            };

            profile.UpdateProfile(profile);
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            return View("~/Views/Admin/AdminDash.cshtml");
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
            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID =  Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            List<NewProjects> NewProjectList = newProjects.GetNewProjects(newProjects.ProfileID);
            ViewBag.ProfileStatus = status.GetProfileStatus(newProjects.ProfileID);

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            return View("~/Views/Client/ClientDashboard.cshtml", NewProjectList);
        }

    }
}
