using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;

using System.Data;

namespace CapstoneProject.Controllers.Reviewer
{
    public class ReviewerController : Controller
    {
        public IActionResult Index()
        {
       
                AdminModel p = new AdminModel();
                //dataset with all profiles and projects
                DataSet projectDs = p.GetProjects();
                //list of project objects and profiles
                List<Project> theProjects = new List<Project>();

                if (projectDs.Tables.Count > 0 && projectDs.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
                {
                    foreach (DataRow row in projectDs.Tables[0].Rows) //each record in the ds
                    {
                        if (row["ProjectStatus"].ToString() == "Pending")
                        {
                            Project project = new Project(); //create a project object for each record
                            project.ProjectID = Convert.ToInt32(row["ProjectID"]);
                            project.ProfileID = Convert.ToInt32(row["ProfileID"]);
                            project.ProjectName = row["ProjectName"].ToString();
                            project.ShortDesc = string.Join(" ", row["ProjectDescription"].ToString().Trim().Split(' ').Take(6));
                            project.Desc = row["ProjectDescription"].ToString();
                            project.ProjectStatus = row["ProjectStatus"].ToString(); // StatusName from TB_Status
                            project.Comments = row["RecentComments"].ToString(); // Latest comment
                            project.SubmittedBy = row["SubmittedBy"].ToString(); // Full name of submitter
                            project.DateSubmitted = Convert.ToDateTime(row["DateSubmitted"]);
                            theProjects.Add(project);
                        }
                    }
                }
               

                ViewBag.AdminViewProjects = theProjects; //viewbag containing the list of projects
                ViewBag.FirstName = HttpContext.Session.GetString("FirstName");


                return View("~/Views/Reviewer/ReviewerDashboard.cshtml");

        }
    }
}
