using CapstoneProject.Attributes;
using CapstoneProject.Models;
using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Data;
//should be used for admin to manage all user profiles
namespace CapstoneProject.Controllers.Admin
{
    public class AdminDashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeRoles("Admin", "Reviewer")]
        public IActionResult Dashboard()
        {
            AdminModel p = new AdminModel();
            Profile profile = new Profile();
            //dataset with all profiles and projects
            DataSet profileDs = profile.GetProfiles();
            DataSet projectDs = p.GetProjects();
            //list of project objects and profiles
            List<Project> theProjects = new List<Project>();
            List<Profile> profiles = new List<Profile>();
            int TotalProjects = 0;
            int ApprovedProjects = 0;
            int RejectedProjects = 0;
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
                    else if (row["ProjectStatus"].ToString() == "Approved")
                    {
                        ApprovedProjects++;
                    }
                    else if (row["ProjectStatus"].ToString() == "Rejected")
                    {
                        RejectedProjects++;
                    }

                    TotalProjects++;
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
            ViewBag.TotalProjects = TotalProjects;
            ViewBag.ApprovedProjects = ApprovedProjects;
            ViewBag.PendingProjects = theProjects.Count;
            ViewBag.RejectedProjects = RejectedProjects;
            return View("~/Views/Dashboard/UserDashboard.cshtml");

        }

        public IActionResult CreateNewProject()
        {


            return View("~/Views/Admin/AdminCreateNewProject.cshtml");
        }

        public IActionResult AddNewProject()
        {

            NewProjects newProjects = new NewProjects();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();
            newProjects.CreateNewProject(10, newProjects.ProfileID, newProjects.ProjectDescription, newProjects.ProjectName);


            return View("~/Views/Admin/AdminCreatedProjectLandingPage.cshtml");
        }

        public IActionResult CancelProject()
        {

            AdminModel p = new AdminModel();
            Profile profile = new Profile();
            //dataset with all profiles and projects
            DataSet profileDs = profile.GetProfiles();
            DataSet projectDs = p.GetProjects();
            //list of project objects and profiles
            List<Project> theProjects = new List<Project>();
            List<Profile> profiles = new List<Profile>();
            int TotalProjects = 0;
            int ApprovedProjects = 0;
            int RejectedProjects = 0;
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
                    else if (row["ProjectStatus"].ToString() == "Approved")
                    {
                        ApprovedProjects++;
                    }
                    else if (row["ProjectStatus"].ToString() == "Rejected")
                    {
                        RejectedProjects++;
                    }

                    TotalProjects++;
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
            ViewBag.TotalProjects = TotalProjects;
            ViewBag.ApprovedProjects = ApprovedProjects;
            ViewBag.PendingProjects = theProjects.Count;
            ViewBag.RejectedProjects = RejectedProjects;
            return View("~/Views/Dashboard/UserDashboard.cshtml");
        }
    }
}
