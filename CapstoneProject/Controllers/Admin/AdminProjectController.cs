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

            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewProjects()
        {
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewAProject(int ProjectID)
        {
            Debug.WriteLine("The projectID for this Project is " + ProjectID); //testing to see that the profileID and projectID is being passed to the view
            
            AdminViewProject proj = new AdminViewProject();
            DataSet ds = new DataSet();

            ds = proj.GetViewedProjectDetails(ProjectID);

            AdminViewProject viewedProject = new AdminViewProject();
            List<AdminViewProject> theProject = new List<AdminViewProject>();

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                    viewedProject.ProjectName = row["ProjectName"].ToString();
                    viewedProject.ProjectDesc = row["ProjectDescription"].ToString();
                    viewedProject.ProjectOwner = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                    viewedProject.Email = row["Email"].ToString();
                    viewedProject.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("MM/dd/yyyy");
                    viewedProject.LastReviewed = Convert.ToDateTime(row["ReviewDate"]).ToString("MM/dd/yyyy");
                    viewedProject.Reviewer = row["ReviewerID"].ToString();

                    int status = Convert.ToInt32(row["ProjectStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 1)
                    {
                        viewedProject.Status = "Approved";
                    }
                    else if (status == 2)
                    {
                        viewedProject.Status = "Pending";
                    }
                    else
                    {
                        viewedProject.Status = "Rejected";
                    }

                    //Get Project Comments
                    Comment c = new Comment();
                    DataSet ds2 = new DataSet();
                    ds2 = c.GetProjectComments(ProjectID);

                    List<Comment> comments = new List<Comment>();

                    if (ds2.Tables[0].Rows.Count > 0 && ds2.Tables.Count > 0) //goes through dataset from GetProjectComments stored procedure
                    {
                        foreach (DataRow row2 in ds2.Tables[0].Rows)
                        {
                            Comment theComment = new Comment();
                            theComment.Description = row2["Comment"].ToString();
                            theComment.StatusChangeDateTime = Convert.ToDateTime(row2["StatusChangeDateTime"]).ToString("MM/dd/yyyy");

                            status = Convert.ToInt32(row2["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                            if (status == 1)
                            {
                                theComment.LastUpdatedStatus = "Approved";
                            }
                            else if (status == 2)
                            {
                                theComment.LastUpdatedStatus = "Pending";
                            }
                            else
                            {
                                theComment.LastUpdatedStatus = "Rejected";
                            }
                            comments.Add(theComment);
                        }
                        viewedProject.Comment = comments;
                    }
                    theProject.Add(viewedProject);
                }

                ViewBag.AdminViewedProject = theProject;
            }
            
            return View("~/Views/Admin/AdminViewProject.cshtml", viewedProject);
        }

        [HttpPost]
        public IActionResult UpdateProjectStatus(int ProjectID, string comment, string status, string date)
        {
            // Log received parameters for debugging
            Debug.WriteLine("Params [ProjectID: " + ProjectID + " Comment: " + comment + " Status: " + status + " Date: " + date + "]");



            return RedirectToAction("ViewAProject", new { ProjectID }); //returns the same view of the viewed project after the update is done
        }
    }
}
