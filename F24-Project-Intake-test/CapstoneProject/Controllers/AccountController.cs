using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using System.Text;
using CapstoneProject.Models;


namespace CapstoneProject.Controllers
{
    public class AccountController : Controller
    {
        string TestAPI_Url = "https://localhost:7151/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account"; //need to change this to the correct URL

        public IActionResult Index()
        {
            return View("~/Views/Account/CreateAccount.cshtml");
        }

        public async Task<IActionResult> CreateAccount(CreateAccountModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Organization))
            {
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            // Manually extract form data for sending via JSON
            Profile profile = new Profile
            {
                FirstName = Request.Form["FirstName"].ToString(),
                LastName = Request.Form["LastName"].ToString(),
                Email = Request.Form["Email"].ToString(),  // Extract email from form   
                Organization = Request.Form["Organization"].ToString(),  // Extract organization from form
                SubmissionDate = DateTime.Now,  // Set the submission date to the current date and time
                Status = "Pending",
                UserType = "Client",
                IsActive = "InActive"
            };

            // Serialize the user object to JSON
            var jsonPayload = JsonSerializer.Serialize(profile);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send the POST request to the API
                    HttpResponseMessage response = await client.PostAsync(TestAPI_Url + "/CreateAccount", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content (user type) as a string
                        string data = await response.Content.ReadAsStringAsync();

                        // Log the user type in the console
                        Console.WriteLine("User Status: " + data);

                        // Pass the user type to the view
                        ViewBag.UserStatus = data;

                        if (ViewBag.UserStatus == "Exists")
                        {
                            ViewBag.ErrorMessage = "Account already exists!";
                            return View();
                        }
                        ViewBag.ErrorMessage = "Account has been submitted for Admin Approval";
                        return View("~/Views/Account/AccountSubmitted.cshtml"); // Redirect to the account submitted page
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "API request failed with status: " + response.StatusCode;
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error: " + ex.Message;
                    return View();
                }
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ResetPasswordClick()
        {
            //do something like put email in db or reset password and email client
            return View("~/Views/Login/Login.cshtml");
        }

        public IActionResult CreateNewProject()
        {


            return View("~/Views/Account/CreateNewProject.cshtml");
        }

        public IActionResult AccountSubmitted()
        {
            return View("~/Views/Account/AccountSubmitted.cshtml");
        }

        public IActionResult AddNewProject()
        {

            NewProjects newProjects = new NewProjects();
            newProjects.ProfileID = Int32.Parse(HttpContext.Session.GetString("ProfileID"));
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();
            newProjects.CreateNewProject(10, newProjects.ProfileID, newProjects.ProjectDescription, newProjects.ProjectName);


            return View("~/Views/Account/CreatedProjectLandingPage.cshtml");
        }

        public IActionResult CancelProject(string searchText = null, string statusFilter = null, string dateRangeFilter = null, DateTime? dateStart = null, DateTime? dateEnd = null)
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

        public IActionResult CreatedProjectLandingPage()
        {
            return View("~/Views/Account/CreatedProjectLandingPage.cshtml");
        }
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            // Redirect to login page or homepage
            return View("~/Views/Login/Login.cshtml");
        }


    }
}
