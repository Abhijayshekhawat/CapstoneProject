using CapstoneProject.Attributes;
using CapstoneProject.Models;
using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Http;
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


            return View("~/Views/Client/ReadOnlyProject.cshtml", viewedProject);
        }
        public IActionResult ClientProfile()
        {
            return View();
        }

        public IActionResult EditClientProfile()
        {

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");
            return View();


        }




        public IActionResult ClientProfileEdit()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
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
            newProjects = newProjects.GetNewProjectByProjectID(ProjectID, newProjects.ProfileID);


            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");

            return View("~/Views/Client/EditClientProject.cshtml", newProjects);





        }

        public IActionResult UpdateClientProject(int ProjectID)
        {
            NewProjects newProjects = new NewProjects();
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();

            newProjects.UpdateClientProject(ProjectID, newProjects.ProjectDescription, newProjects.ProjectName);


            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            List<NewProjects> NewProjectList = newProjects.GetNewProjects(newProjects.ProfileID);
            ViewBag.ProfileStatus = status.GetProfileStatus(newProjects.ProfileID);

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");

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
                ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID")), //need to double check this
                UserType = Request.Form["UserType"].ToString()
            };
            HttpContext.Session.SetString("Organization", Request.Form["Organization"].ToString());
            HttpContext.Session.SetString("UserType", Request.Form["UserType"].ToString());
            HttpContext.Session.SetString("FirstName", Request.Form["FirstName"].ToString());
            HttpContext.Session.SetString("LastName", Request.Form["LastName"].ToString());

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
            ViewBag.Organization = HttpContext.Session.GetString("Organization");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.SubmissionDate = HttpContext.Session.GetString("SubmissionDate");



            return View("~/Views/Client/ClientProfileEdit.cshtml");
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
        public IActionResult ClientDashboard(string searchText = null, string statusFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            NewProjects newProjects = new NewProjects();
            ProfileStatus status = new ProfileStatus();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            List<NewProjects> NewProjectList = newProjects.GetNewProjects(newProjects.ProfileID);

            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                NewProjectList = NewProjectList
                    .Where(p => p.ProjectName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                NewProjectList = NewProjectList
                    .Where(p => p.ProjectStatus.Equals(statusFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply date range filter
            // Apply date range filter
            DateTime today = DateTime.Today;
            switch (dateRangeFilter)
            {
                case "today":
                    NewProjectList = NewProjectList.Where(p => p.Submissiondate.Date == today).ToList();
                    break;
                case "week":
                    DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    NewProjectList = NewProjectList.Where(p => p.Submissiondate.Date >= startOfWeek && p.Submissiondate.Date <= today).ToList();
                    break;
                case "month":
                    DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
                    NewProjectList = NewProjectList.Where(p => p.Submissiondate.Date >= startOfMonth && p.Submissiondate.Date <= today).ToList();
                    break;
                case "custom":
                    if (dateStart.HasValue)
                    {
                        NewProjectList = NewProjectList.Where(p => p.Submissiondate >= dateStart.Value).ToList();
                    }
                    if (dateEnd.HasValue)
                    {
                        NewProjectList = NewProjectList.Where(p => p.Submissiondate <= dateEnd.Value).ToList();
                    }
                    break;
            }
            ViewBag.ProfileStatus = status.GetProfileStatus(newProjects.ProfileID);

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            ViewBag.ApprovedProjects = NewProjectList.Count(p => p.ProjectStatus == "Approved");
            ViewBag.PendingProjects = NewProjectList.Count(p => p.ProjectStatus == "Pending");
            ViewBag.RejectedProjects = NewProjectList.Count(p => p.ProjectStatus == "Rejected");
            return View("~/Views/Client/ClientDashboard.cshtml", NewProjectList);
        }

    }
}
