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

        public IActionResult ClientProfileEdit()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");
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
