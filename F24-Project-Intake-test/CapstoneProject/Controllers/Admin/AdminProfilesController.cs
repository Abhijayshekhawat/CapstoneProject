using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static CapstoneProject.Models.AdminModel;
using System.Diagnostics;
using Profile = CapstoneProject.Models.ClassLibrary.Profile;
using System.Data.Common;
using CapstoneProject.Models.Utilities;
using Microsoft.AspNetCore.Identity;
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
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");
            return View("~/Views/Admin/AdminManageProfiles.cshtml", profiles);
        }

        public IActionResult ViewAProfile(int ProfileID)
        {
            Debug.WriteLine("The profileID for this profile is " + ProfileID); //testing to see that the profileID and projectID is being passed to the view
            ViewBag.ProfileID = ProfileID;
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
                    viewedProfile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("g");
                    viewedProfile.StatusChangeDateTime = Convert.ToDateTime(row["StatusChangeDateTime"]).ToString("g");
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
                    int activeUser = row["IsActive"] != DBNull.Value ? Convert.ToInt32(row["IsActive"]) : -1;
                    //user type is stored as an int in db, for the admin view we want to show the string
                    if (activeUser == 1)
                    {
                        viewedProfile.IsActive = "Active";
                    }
                    else if (activeUser == 2)
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
                            Profile Commenter = new Profile();
                            Commenter = Commenter.GetCommenterDetails(Convert.ToInt32(row2["CommenterID"].ToString()));
                            string commenter = Commenter.FirstName + " " + Commenter.LastName;
                            Comment theComment = new Comment();
                            theComment.Description = row2["Comment"].ToString();
                            theComment.StatusChangeDateTime = Convert.ToDateTime(row2["StatusChangeDateTime"]).ToString("g");
                            theComment.CommenterName = commenter;
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
                            viewedProfile.LastReviewed = Convert.ToDateTime(row2["StatusChangeDateTime"]).ToString("g");
                            viewedProfile.Reviewer = commenter;
                        }
                        viewedProfile.Comments = comments;
                    }

                    theProfile.Add(viewedProfile);
                }
                ViewBag.AdminViewedProfile = theProfile[0];
                ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
                ViewBag.LastName = HttpContext.Session.GetString("LastName");
                ViewBag.UserType = HttpContext.Session.GetString("UserType");
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

        [HttpPost]
        public IActionResult UpdateProfile()
        {
            // Initialize the comment text
            int profileID = Convert.ToInt32(Request.Form["ProfileID"]);
            string firstName = Request.Form["FirstName"];
            string lastName = Request.Form["LastName"];
            string organization = Request.Form["Organization"];
            string email = Request.Form["Email"];
            string isActive = string.IsNullOrEmpty(Request.Form["IsActive"]) ? "User not created" : Request.Form["IsActive"];
            string userType = Request.Form["UserType"];
            string comment = Request.Form["Comment"];
            string status = Request.Form["Status"];

            Debug.WriteLine("The profileID for this profile is " + profileID); //testing to see that the profileID and projectID is being passed to the view

            ViewedProfile p = new ViewedProfile();
            DataSet ds = new DataSet();

            ds = p.GetViewedProfileDetails(profileID);

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
                    viewedProfile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("g");
                    viewedProfile.StatusChangeDateTime = Convert.ToDateTime(row["StatusChangeDateTime"]).ToString("g");
                    viewedProfile.Comment = row["Comment"].ToString();

                    int status1 = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status1 == 1)
                    {
                        viewedProfile.Status = "Approved";
                    }
                    else if (status1 == 2)
                    {
                        viewedProfile.Status = "Pending";
                    }
                    else if (status1 == 3)
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
                    int activeUser = row["IsActive"] != DBNull.Value ? Convert.ToInt32(row["IsActive"]) : -1;
                    //user type is stored as an int in db, for the admin view we want to show the string
                    if (activeUser == 1)
                    {
                        viewedProfile.IsActive = "Active";
                    }
                    else if (activeUser == 2)
                    {
                        viewedProfile.IsActive = "Inactive";
                    }
                    else
                    {
                        viewedProfile.IsActive = "User not created";
                    }


                    theProfile.Add(viewedProfile);
                }
                ViewBag.AdminViewedProfile = theProfile[0];
            }

            var changeLog = new List<string>();

            // Check each field for changes and log them
            if (!string.Equals(theProfile[0].FirstName, firstName, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"First Name was changed from '{theProfile[0].FirstName}' to '{firstName}'");
            }

            if (!string.Equals(theProfile[0].LastName, lastName, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"Last Name was changed from '{theProfile[0].LastName}' to '{lastName}'");
            }

            if (!string.Equals(theProfile[0].Organization, organization, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"Organization was changed from '{theProfile[0].Organization}' to '{organization}'");
            }

            if (!string.Equals(theProfile[0].Email, email, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"Email was changed from '{theProfile[0].Email}' to '{email}'");
            }

            if (!string.Equals(theProfile[0].IsActive, isActive, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"Activation Status was changed from '{theProfile[0].IsActive}' to '{isActive}'");
            }

            if (!string.Equals(theProfile[0].UserType, userType, StringComparison.OrdinalIgnoreCase))
            {
                changeLog.Add($"User Type was changed from '{theProfile[0].UserType}' to '{userType}'");
            }

            // Append the generated change log to the provided comment
            string concatenatedComment = string.Join(". ", changeLog);
            if (!string.IsNullOrWhiteSpace(comment))
            {
                concatenatedComment = $"{comment}. {concatenatedComment}";
            }

            //profile + user update
            Profile updateProfile = new Profile();
            updateProfile.Organization = organization;
            updateProfile.FirstName = firstName;
            updateProfile.LastName = lastName;
            updateProfile.Email = email;
            updateProfile.ProfileID = profileID;
            updateProfile.UserType = userType;
            updateProfile.IsActive = isActive;
            int s = 0;
            if (status.Equals("Approved"))
            {
                s = 1;
            }
            else if (status.Equals("Pending"))
            {
                s = 2;
            }
            else if (status.Equals("Rejected"))
            {
                s = 3;
            }
            if (theProfile[0].IsActive != "User not created")
            {
                updateProfile.UpdateProfile(updateProfile);
            }
            else
            {
                if (s == 1)
                {
                    int uType = 0;
                    if (updateProfile.UserType == "Client")
                    {
                        uType = 1;
                    }
                    else if (updateProfile.UserType == "Reviewer")
                    {
                        uType = 2;
                    }
                    else if (updateProfile.UserType == "Admin")
                    {
                        uType = 3;
                    }
                    User user = new User();
                    if (user.CreateUser(profileID, updateProfile.Email, updateProfile.FirstName, updateProfile.LastName, uType, true, out string tempPassword))
                    {
                        string temp = tempPassword;
                        //send client Email about new account
                        Email objEmail = new Email();
                        String strTO = updateProfile.Email;
                        String strFROM = "templeprojectintake@gmail.com";
                        String strSubject = "Temple Project Intake - Account Created";
                        String strMessage = "";
                        if (strTO.Contains("temple.edu"))
                        {
                            strMessage = $@"
                                                <html>
                                                <head>
                                                    <style>
                                                        body {{
                                                            font-family: Arial, sans-serif;
                                                            background-color: #f4f4f9;
                                                            color: #333;
                                                            margin: 0;
                                                            padding: 0;
                                                        }}
                                                        .email-container {{
                                                            max-width: 600px;
                                                            margin: 20px auto;
                                                            background: #ffffff;
                                                            border: 1px solid #e0e0e0;
                                                            border-radius: 8px;
                                                            padding: 0;
                                                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                                                        }}
                                                        .header {{
                                                            background-color: #A41E35;
                                                            color: white;
                                                            padding: 15px;
                                                            text-align: center;
                                                            border-radius: 8px 8px 0 0;
                                                        }}
                                                        .header img {{
                                                            height: 50px;
                                                            margin-bottom: 10px;
                                                        }}
                                                        .content {{
                                                            padding: 20px;
                                                        }}
                                                        .credentials {{
                                                            margin: 20px 0;
                                                            padding: 15px;
                                                            background-color: #f9f9f9;
                                                            border: 1px dashed #cccccc;
                                                            border-radius: 5px;
                                                        }}
                                                        .footer {{
                                                            background-color: #f4f4f9;
                                                            padding: 15px;
                                                            text-align: center;
                                                            font-size: 0.9em;
                                                            color: #777;
                                                            border-top: 1px solid #e0e0e0;
                                                            border-radius: 0 0 8px 8px;
                                                        }}
                                                    </style>
                                                </head>
                                                <body>
                                                    <div class='email-container'>
                                                        <div class='header'>
                                                            <img src='https://liberalarts.temple.edu/sites/all/modules/custom/tu_layout/theme/img/temple-logo-t-box.svg' alt='Temple Logo' />
                                                            <h1>Temple Project Intake</h1>
                                                        </div>
                                                        <div class='content'>
                                                            <p>Hi {updateProfile.FirstName},</p>
                                                            <p>Your account has been successfully created. You can now log in using your Temple Account:</p>
                                                            <div class='credentials'>
                                                                <p><strong>Email:</strong> {updateProfile.Email}</p>
                                                            </div>
                                                            <p><strong>Important:</strong> Please login through the SSO option using your Accessnet username and password.</p>
                                                            <p>If you have any questions or need assistance, feel free to contact Temple University.</p>
                                                            <p>Thank you,<br>The Temple Project Intake Team</p>
                                                        </div>
                                                        <div class='footer'>
                                                            <p>&copy; {DateTime.Now.Year} Temple Project Intake. All rights reserved.</p>
                                                        </div>
                                                    </div>
                                                </body>
                                                </html>";
                        }
                        else
                        {
                            strMessage = $@"
                                                <html>
                                                <head>
                                                    <style>
                                                        body {{
                                                            font-family: Arial, sans-serif;
                                                            background-color: #f4f4f9;
                                                            color: #333;
                                                            margin: 0;
                                                            padding: 0;
                                                        }}
                                                        .email-container {{
                                                            max-width: 600px;
                                                            margin: 20px auto;
                                                            background: #ffffff;
                                                            border: 1px solid #e0e0e0;
                                                            border-radius: 8px;
                                                            padding: 0;
                                                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                                                        }}
                                                        .header {{
                                                            background-color: #A41E35;
                                                            color: white;
                                                            padding: 15px;
                                                            text-align: center;
                                                            border-radius: 8px 8px 0 0;
                                                        }}
                                                        .header img {{
                                                            height: 50px;
                                                            margin-bottom: 10px;
                                                        }}
                                                        .content {{
                                                            padding: 20px;
                                                        }}
                                                        .credentials {{
                                                            margin: 20px 0;
                                                            padding: 15px;
                                                            background-color: #f9f9f9;
                                                            border: 1px dashed #cccccc;
                                                            border-radius: 5px;
                                                        }}
                                                        .footer {{
                                                            background-color: #f4f4f9;
                                                            padding: 15px;
                                                            text-align: center;
                                                            font-size: 0.9em;
                                                            color: #777;
                                                            border-top: 1px solid #e0e0e0;
                                                            border-radius: 0 0 8px 8px;
                                                        }}
                                                    </style>
                                                </head>
                                                <body>
                                                    <div class='email-container'>
                                                        <div class='header'>
                                                            <img src='https://liberalarts.temple.edu/sites/all/modules/custom/tu_layout/theme/img/temple-logo-t-box.svg' alt='Temple Logo' />
                                                            <h1>Temple Project Intake</h1>
                                                        </div>
                                                        <div class='content'>
                                                            <p>Hi {updateProfile.FirstName},</p>
                                                            <p>Your account has been successfully created. You can now log in using the credentials provided below:</p>
                                                            <div class='credentials'>
                                                                <p><strong>Email:</strong> {updateProfile.Email}</p>
                                                                <p><strong>Temporary Password:</strong> {temp}</p>
                                                            </div>
                                                            <p><strong>Important:</strong> Please change your password immediately after logging in to secure your account.</p>
                                                            <p>If you have any questions or need assistance, feel free to contact Temple University.</p>
                                                            <p>Thank you,<br>The Temple Project Intake Team</p>
                                                        </div>
                                                        <div class='footer'>
                                                            <p>&copy; {DateTime.Now.Year} Temple Project Intake. All rights reserved.</p>
                                                        </div>
                                                    </div>
                                                </body>
                                                </html>";
                        }
                        try
                        {
                            objEmail.SendMail(strTO, strFROM, strSubject, strMessage); // Ensure isHtml is set to true
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to send verification email: {ex.Message}");
                            ViewBag.ErrorMessage = "The email wasn't sent because: " + ex.Message;
                        }
                    }


                }
            }
            //comment + status update
            ProfileStatus update = new ProfileStatus();
            int commenterID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            update.UpdateProfileStatus(profileID, s, concatenatedComment);
            update.AddProfileComment(commenterID, profileID, s, concatenatedComment);

            return RedirectToAction("Dashboard", "AdminDash");

        }

    }
}
