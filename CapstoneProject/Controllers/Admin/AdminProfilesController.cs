using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static CapstoneProject.Models.AdminModel;
using System.Diagnostics;
using Profile = CapstoneProject.Models.ClassLibrary.Profile;
//should be used for admin to manage all user profiles
namespace CapstoneProject.Controllers.Admin
{
    public class AdminProfilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageProfiles(string searchText = null, string statusFilter = null, string userTypeFilter = null, string userActiveFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            //Use stored procedure to get profile data from datatable
            //create profile objects and add them to the viewbag

            Profile p = new Profile();
            DataSet ds = new DataSet();

            ds = p.GetProfiles();

            List<Profile> profiles = new List<Profile>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                    Profile userProfile = new Profile();
                    userProfile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    userProfile.FirstName = row["FirstName"].ToString();
                    userProfile.LastName = row["LastName"].ToString();
                    userProfile.Organization = row["Organization"].ToString();
                    userProfile.Email = row["Email"].ToString();
                    if (row["LastUpdatedStatus"].Equals(1))
                    {
                        userProfile.Status = "Approved";
                    }
                    else if (row["LastUpdatedStatus"].Equals(2))
                    {
                        userProfile.Status = "Pending";
                    }
                    else if (row["LastUpdatedStatus"].Equals(3))
                    {
                        userProfile.Status = "Rejected";
                    }
                    userProfile.SubmissionDate = DateTime.Parse(row["SubmissionDate"].ToString());
                    userProfile.UserType = row["UserType"].ToString();
                    userProfile.IsActive = row["UserActiveStatus"].ToString();
                    profiles.Add(userProfile);
                }
            }
            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                profiles = profiles
                    .Where(p => p.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Organization.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                profiles = profiles
                    .Where(p => p.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(userTypeFilter))
            {
                profiles = profiles
                    .Where(p => p.UserType.Equals(userTypeFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(userActiveFilter))
            {
                profiles = profiles
                    .Where(p => p.IsActive.Equals(userActiveFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            //Apply date range filter
            DateTime today = DateTime.Today;
            switch (dateRangeFilter)
            {
                case "today":
                    profiles = profiles.Where(p => p.SubmissionDate.Date == today).ToList();
                    break;
                case "week":
                    DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    profiles = profiles.Where(p => p.SubmissionDate.Date >= startOfWeek && p.SubmissionDate.Date <= today).ToList();
                    break;
                case "month":
                    DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
                    profiles = profiles.Where(p => p.SubmissionDate.Date >= startOfMonth && p.SubmissionDate.Date <= today).ToList();
                    break;
                case "custom":
                    if (dateStart.HasValue)
                    {
                        profiles = profiles.Where(p => p.SubmissionDate >= dateStart.Value).ToList();
                    }
                    if (dateEnd.HasValue)
                    {
                        profiles = profiles.Where(p => p.SubmissionDate <= dateEnd.Value).ToList();
                    }
                    break;
            }
            ViewBag.AdminViewProfiles = profiles; //viewbag containing the profiles

            return View("~/Views/Admin/AdminManageProfiles.cshtml", profiles);
        }

        public IActionResult ViewAProfile(int ProfileID)
        {
            Debug.WriteLine("The profileID for this profile is " + ProfileID); //testing to see that the profileID and projectID is being passed to the view

            ViewedProfile p = new ViewedProfile();
            DataSet ds = new DataSet();

            ds = p.GetViewedProfileDetails(ProfileID);

            ViewedProfile viewedProfile = new ViewedProfile();
            List<ViewedProfile> theProfile = new List<ViewedProfile>();

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                    viewedProfile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    viewedProfile.FirstName = row["FirstName"].ToString();
                    viewedProfile.LastName = row["LastName"].ToString();
                    viewedProfile.Organization = row["Organization"].ToString();
                    viewedProfile.Email = row["Email"].ToString();
                    viewedProfile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("MM/dd/yyyy");
                    viewedProfile.StatusChangeDateTime = Convert.ToDateTime(row["StatusChangeDateTime"]).ToString("MM/dd/yyyy");
                    viewedProfile.Comment = row["Comment"].ToString();

                    int status = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 1)
                    {
                        viewedProfile.Status = "Approved";
                    }
                    else if (status == 2)
                    {
                        viewedProfile.Status = "Pending";
                    }
                    else
                    {
                        viewedProfile.Status = "Rejected";
                    }
                    int uType = row["UserType"] != DBNull.Value ? Convert.ToInt32(row["UserType"]) : -1;
                    //user type is stored as an int in db, for the admin view we want to show the string
                    if (uType == 1)
                    {
                        viewedProfile.UserType = "Client";
                    }
                    else if (uType == 2)
                    {
                        viewedProfile.UserType = "Reviewer";
                    }
                    else if (uType == 3)
                    {
                        viewedProfile.UserType = "Admin";
                    }
                    else
                    {
                        viewedProfile.UserType = "Unassigned";
                    }
                    int activeUser = row["IsActive"] != DBNull.Value ? Convert.ToInt32(row["UserType"]) : -1;
                    //user type is stored as an int in db, for the admin view we want to show the string
                    if (uType == 1)
                    {
                        viewedProfile.IsActive = "Active";
                    }
                    else if (uType == 2)
                    {
                        viewedProfile.IsActive = "Inactive";
                    }
                    else
                    {
                        viewedProfile.IsActive = "User not created";
                    }
                    //Get Profile Comments
                    Comment c = new Comment();
                    DataSet ds2 = new DataSet();
                    ds2 = c.GetProfileComments(ProfileID);

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
                        viewedProfile.Comments = comments;
                    }

                    theProfile.Add(viewedProfile);
                }
                ViewBag.AdminViewedProfile = theProfile;
            }

            return View("~/Views/Admin/AdminViewProfile.cshtml", viewedProfile);
        }

        [HttpPost]
        public IActionResult UpdateProfileStatus(int ProfileID, string comment, string status)
        {
            int s;
            if (status.Equals("Approved"))
            {
                s = 1;
            }
            else if (status.Equals("Pending"))
            {
                s = 2;
            }
            else
            {
                s = 3;
            }

            ProfileStatus update = new ProfileStatus();
            int commenterID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            update.UpdateProfileStatus(ProfileID, s, comment);
            update.AddProfileComment(commenterID, ProfileID, s, comment);

            return RedirectToAction("ViewAProfile", new { ProfileID }); //returns the same view of the viewed profile after the update is done
        }

        [HttpPost]
        public IActionResult UpdateProfileUserType(int ProfileID, string UserType)
        {
            if (UserType.Equals("Unassigned", StringComparison.OrdinalIgnoreCase))
            {
                // Prevent the update and return an error message or redirect
                TempData["Error"] = "Cannot update User Type for Unassigned users.";
                return RedirectToAction("ViewAProfile", new { ProfileID });
            }

            int u;
            if (UserType.Equals("Client", StringComparison.OrdinalIgnoreCase))
            {
                u = 1;
            }
            else if (UserType.Equals("Reviewer", StringComparison.OrdinalIgnoreCase))
            {
                u = 2;
            }
            else
            {
                u = 3;
            }

            User update = new User();
            update.ChangeUserType(ProfileID, u);

            TempData["Success"] = "User Type updated successfully.";
            return RedirectToAction("ViewAProfile", new { ProfileID });
        }

    }
}
