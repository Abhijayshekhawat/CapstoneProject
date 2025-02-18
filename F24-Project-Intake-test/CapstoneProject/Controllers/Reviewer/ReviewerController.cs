using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;

using System.Data;
using System.Diagnostics;

namespace CapstoneProject.Controllers.Reviewer
{
    public class ReviewerController : Controller
    {
        public IActionResult Index(string searchText = null, string statusFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            AdminModel p = new AdminModel();
            //dataset with all profiles and projects
            DataSet projectDs = p.GetProjects();
            //list of project objects and profiles
            List<Project> theProjects = new List<Project>();
            int count = 0;
            int pending = 0;
            int approved = 0;
            int rejected = 0;
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
                        pending++;
                    }
                    else if (row["ProjectStatus"].ToString() == "Approved")
                    {
                        approved++;
                    }
                    else if (row["ProjectStatus"].ToString() == "Rejected")
                    {
                        rejected++;
                    }
                    count++;
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
            ViewBag.ApprovedProjects = approved;
            ViewBag.PendingProjects = pending;
            ViewBag.RejectedProjects = rejected;
            ViewBag.TotalProjects = count;

            return View("~/Views/Reviewer/ReviewerDashboard.cshtml");

        }


        public IActionResult UpdateReviewerProfile()
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
            AdminModel p = new AdminModel();

            //dataset with all profiles and projects
            DataSet profileDs = profile.GetProfiles();
            DataSet projectDs = p.GetProjects();
            //list of project objects and profiles
            List<Project> theProjects = new List<Project>();
            List<Profile> profiles = new List<Profile>();

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
            if (profileDs.Tables.Count > 0 && profileDs.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in profileDs.Tables[0].Rows) //each record in the ds
                {
                    Profile userProfile = new Profile();
                    userProfile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    userProfile.FirstName = row["FirstName"].ToString();
                    userProfile.LastName = row["LastName"].ToString();
                    userProfile.Organization = row["Organization"].ToString();
                    userProfile.Email = row["Email"].ToString();

                    if (row["LastUpdatedStatus"].Equals(DBNull.Value))
                    {
                        userProfile.Status = "NULL Value";
                        profiles.Add(userProfile);
                    }
                    else
                    {
                        int status = Convert.ToInt32(row["LastUpdatedStatus"]);

                        if (status == 2)
                        {
                            userProfile.Status = "Pending";
                            profiles.Add(userProfile);
                        }
                    }
                }

            }

            ViewBag.AdminViewProfiles = profiles; //viewbag containing the profiles
            ViewBag.AdminViewProjects = theProjects; //viewbag containing the list of projects
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");




            return View("~/Views/Reviewer/ReviewerDashboard.cshtml");
        }

        public IActionResult ReviewerManageProject(string searchText = null, string statusFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
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

            return View("~/Views/Reviewer/ReviewerManageProject.cshtml");
        }
        public IActionResult ReviewerViewProjects()
        {
            return View("~/Views/Reviewer/ReviewerManageProject.cshtml");
        }

        public IActionResult ReviewerViewAProject(int ProjectID)
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
                    viewedProject.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("");
                    if (row["ReviewDate"] != DBNull.Value)
                    {
                        viewedProject.LastReviewed = Convert.ToDateTime(row["ReviewDate"]).ToString("MM/dd/yyyy");
                    }

                    if (row["ReviewerID"] != DBNull.Value)
                    {
                        viewedProject.Reviewer = row["ReviewerID"].ToString();
                    }


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
                            theComment.StatusChangeDateTime = Convert.ToDateTime(row2["StatusChangeDateTime"]).ToString();

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
                ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
                ViewBag.LastName = HttpContext.Session.GetString("LastName");
                ViewBag.UserType = HttpContext.Session.GetString("UserType");
            }

            return View("~/Views/Reviewer/ReviewerViewAProject.cshtml", viewedProject);
        }
        public IActionResult ReviewerEditProfile()
        {

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");
            return View();


        }


    }

}
