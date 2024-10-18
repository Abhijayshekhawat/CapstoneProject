using CapstoneProject.Models;
using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
//should be used for admin to manage all projects
namespace CapstoneProject.Controllers.Admin
{
    public class AdminProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageProjects()
        {
            //Use stored procedure to get project data from datatable
            //create project objects and add them to the viewbag

            AdminModel p = new AdminModel();
            DataSet ds = new DataSet();

            //dataset with all the projects
            ds = p.GetProjects();

            List<Project> theProjects = new List<Project>(); //list of project objects

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                    Project project = new Project(); //create a project object for each record
                    project.ProjectID = Convert.ToInt32(row["ProjectID"]);
                    project.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    project.ProjectName = row["ProjectName"].ToString();
                    project.ShortDesc = row["ProjectDescription"].ToString();

                    int status = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 1)
                    {
                        project.ProjectStatus = "Approved";
                    }
                    else if (status == 2)
                    {
                        project.ProjectStatus = "Pending";
                    }
                    else
                    {
                        project.ProjectStatus = "Rejected";
                    }
                    project.Comments = row["Comment"].ToString();
                    theProjects.Add(project);
                }
            }
            
            ViewBag.AdminViewProjects = theProjects; //viewbag containing the list of projects

            return View("~/Views/Admin/AdminManageProjects.cshtml", theProjects);
        }

        public IActionResult ViewProjects()
        {
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewAProject()
        {
            return View("~/Views/Admin/AdminViewProject.cshtml");
        }
    }
}
