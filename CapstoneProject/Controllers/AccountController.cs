using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult CreateNewProject()
        {


            return View("~/Views/Account/CreateNewProject.cshtml");
        }

        public IActionResult AddNewProject() 
        {

            NewProjects newProjects = new NewProjects();    
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();
            newProjects.CreateNewProject(newProjects.ProjectName, newProjects.ProjectDescription);
        
            return View("~/Views/Account/ClientProject.cshtml", newProjects);
        }



    }
}
