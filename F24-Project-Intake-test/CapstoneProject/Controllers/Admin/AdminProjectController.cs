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

        public IActionResult ManageProjects(string searchText = null, string statusFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
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
                    project.ShortDesc = string.Join(" ", row["ProjectDescription"].ToString().Trim().Split(' ').Take(6));
                    project.Desc = row["ProjectDescription"].ToString();
                    project.ProjectStatus = row["ProjectStatus"].ToString(); // StatusName from TB_Status
                    project.Comments = row["RecentComments"].ToString(); // Latest comment
                    project.SubmittedBy = row["SubmittedBy"].ToString(); // Full name of submitter
                    project.DateSubmitted = Convert.ToDateTime(row["DateSubmitted"]);
                    theProjects.Add(project);
                }
            }
            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                theProjects = theProjects
                    .Where(p => p.ProjectName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                theProjects = theProjects
                    .Where(p => p.ProjectStatus.Equals(statusFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply date range filter
            // Apply date range filter
            DateTime today = DateTime.Today;
            switch (dateRangeFilter)
            {
                case "today":
                    theProjects = theProjects.Where(p => p.DateSubmitted.Date == today).ToList();
                    break;
                case "week":
                    DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    theProjects = theProjects.Where(p => p.DateSubmitted.Date >= startOfWeek && p.DateSubmitted.Date <= today).ToList();
                    break;
                case "month":
                    DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
                    theProjects = theProjects.Where(p => p.DateSubmitted.Date >= startOfMonth && p.DateSubmitted.Date <= today).ToList();
                    break;
                case "custom":
                    if (dateStart.HasValue)
                    {
                        theProjects = theProjects.Where(p => p.DateSubmitted >= dateStart.Value).ToList();
                    }
                    if (dateEnd.HasValue)
                    {
                        theProjects = theProjects.Where(p => p.DateSubmitted <= dateEnd.Value).ToList();
                    }
                    break;
            }

            ViewBag.AdminViewProjects = theProjects; //viewbag containing the list of projects
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewProjects()
        {
            return View("~/Views/Admin/AdminManageProjects.cshtml");
        }

        public IActionResult ViewAProject(int ProjectID)
        {
            Debug.WriteLine("The projectID for this Project is " + ProjectID); // Testing projectID

            // Create a new AdminViewProject instance
            AdminViewProject viewedProject = new AdminViewProject();

            // Fetch project details from the database
            DataSet ds = viewedProject.GetViewedProjectDetails(ProjectID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0]; // Fetch the first (and only) row

                // Populate the viewedProject object
                viewedProject.ProjectID = Convert.ToInt32(row["ProjectID"].ToString());
                viewedProject.ProjectName = row["ProjectName"].ToString();
                viewedProject.ProjectDesc = row["ProjectDescription"].ToString();
                viewedProject.ProjectOwner = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                viewedProject.Email = row["Email"].ToString();
                viewedProject.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("g");

                if (row["ReviewDate"] != DBNull.Value)
                {
                    viewedProject.LastReviewed = Convert.ToDateTime(row["ReviewDate"]).ToString("g");
                }

                if (row["ReviewerID"] != DBNull.Value)
                {
                    viewedProject.Reviewer = row["ReviewerID"].ToString();
                    Profile rev = new Profile();
                    ViewBag.ReviewerDetails = rev.GetProjectReviewerDetails(Convert.ToInt32(viewedProject.Reviewer));
                }

                int status = Convert.ToInt32(row["ProjectStatus"]);
                viewedProject.Status = status switch
                {
                    1 => "Approved",
                    2 => "Pending",
                    _ => "Rejected"
                };

                // Fetch project comments
                Comment c = new Comment();
                DataSet ds2 = c.GetProjectComments(ProjectID);
                List<Comment> comments = new List<Comment>();
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row2 in ds2.Tables[0].Rows)
                    {
                        Profile p = new Profile();
                        p = p.GetCommenterDetails(Convert.ToInt32(row2["CommenterID"].ToString()));
                        string commenter = p.FirstName + " " + p.LastName;
                        Comment theComment = new Comment
                        {
                            CommenterName = commenter,
                            Description = row2["Comment"].ToString(),
                            StatusChangeDateTime = Convert.ToDateTime(row2["StatusChangeDateTime"]).ToString("g"),
                            LastUpdatedStatus = Convert.ToInt32(row2["LastUpdatedStatus"]) switch
                            {
                                1 => "Approved",
                                2 => "Pending",
                                _ => "Rejected"
                            }
                        };
                        comments.Add(theComment);
                    }
                }
                viewedProject.Comment = comments;
            }

            // Pass the single project object to the view
            ViewBag.AdminViewedProject = viewedProject;
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            // Return the view
            return View("~/Views/Admin/AdminViewProject.cshtml");
        }



        [HttpPost]
        public IActionResult UpdateProjectStatus(int ProjectID, string comment, string status)
        {
            int newstatus;
            ProjectStatus update = new ProjectStatus();
            int ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            if (status.Equals("Approved"))
            {
                newstatus = 1;



            }
            else if (status.Equals("Pending"))
            {
                newstatus = 2;

            }
            else
            {
                newstatus = 3;







            }

            update.UpdateProjectStatus(ProfileID, ProjectID, newstatus); //UPDATES TB_NewProjects Changing the status and Review Date

            update.AddProjectComment(ProfileID, ProjectID, comment, newstatus); //Insert new comment into TB_Comments

            update.AddCommentToProjectStatus(ProjectID, comment, newstatus); //Insert new comment into TB_ProjectStatus



            return RedirectToAction("ViewAProject", new { ProjectID }); //returns the same view of the viewed project after the update is done
        }
    }
}
