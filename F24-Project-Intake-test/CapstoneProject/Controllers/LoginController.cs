using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;
using CapstoneProject.Models;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CapstoneProject.Models.Utilities;
using System.Text;
using CapstoneProject.Attributes;
using System.Net.Mail;


namespace CapstoneProject.Controllers
{
    public class LoginController : Controller
    {
        string TestAPI_Url = "https://localhost:7151/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account"; //need to change this to the correct URL
        private readonly EncryptionHelper _encryptionHelper;
        public LoginController(EncryptionHelper encryptionHelper)
        {
            _encryptionHelper = encryptionHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Submit(string email, string password)
        {
            // You can add logic here to validate the email and password if needed.
            // For now, it redirects to the Home page after login.
            return View("~/Views/Home/index.cshtml");
        }
        [HttpPost]
        public IActionResult Sso()
        {
            // This can redirect to a secure area after successful SSO login.
            return View("~/Views/Secure/index.cshtml");
        }
        public async Task<IActionResult> Login(LoginModel model)
        {
            //Email Part
            //Email objEmail = new Email();
            //String strTO = "abhijay.shekhawat@temple.edu";
            //String strFROM = "templeprojectintake@gmail.com";
            //String strSubject = "Email Functionality";
            //String strMessage = "Hi Johnson, I figured out the email part. Who's the goat?";
            ////**** Uncomment everything before return view  when you want to send the email 

            //try
            //{
            //    objEmail.TryMail(strTO, strFROM, strSubject, strMessage);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Failed to send verification email: {ex.Message}");
            //    ViewBag.ErrorMessage = "The email wasn't sent because: " + ex.Message;
            //}
            // Manually check if Email and Password fields are populated
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return View(model);
            }
            // Use LoginModel for validation purposes
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }
            if (Request.Form["Email"].ToString().Contains("temple.edu"))
            {
                ViewBag.ErrorMessage = "Temple-affiliated email detected. Please use the SSO option to sign in.";
                return View();
            }
            // Manually extract form data for sending via JSON
            User user = new User
            {
                FirstName = "",
                LastName = "",
                UserType = "",
                Email = Request.Form["Email"].ToString(),  // Extract email from form
                PasswordHash = EncryptionHelper.ComputeHash(Request.Form["Password"].ToString())  // Extract password from form
            };

            // Serialize the user object to JSON
            var jsonPayload = JsonSerializer.Serialize(user);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send the POST request to the API
                    HttpResponseMessage response = await client.PostAsync(TestAPI_Url + "/Login", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content (user type) as a string
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var profile = JsonSerializer.Deserialize<Profile>(responseBody, options);

                        if (profile != null && profile.FirstName == "Inactive")
                        {
                            ViewBag.ErrorMessage = "Your account is Inactive. Please reach out to the administrator to reactivate it!";
                            return View();
                        }
                        if (profile != null && !string.IsNullOrEmpty(profile.FirstName) && !string.IsNullOrEmpty(profile.LastName))
                        {
                            HttpContext.Session.SetString("Organization", profile.Organization);
                            HttpContext.Session.SetString("UserType", profile.UserType);
                            HttpContext.Session.SetString("FirstName", profile.FirstName);
                            HttpContext.Session.SetString("LastName", profile.LastName);
                            HttpContext.Session.SetString("Email", profile.Email);
                            HttpContext.Session.SetString("SubmissionDate", profile.SubmissionDate.ToString("MM/dd/yyyy"));
                            HttpContext.Session.SetString("ProfileID", profile.ProfileID.ToString());

                            // Redirect based on UserType
                            if (profile.UserType == "Client")
                            {
                                return RedirectToAction("ClientDashboard", "Client");
                            }
                            else if (profile.UserType == "Admin")
                            {
                                return RedirectToAction("Dashboard", "AdminDash");
                            }
                            else if (profile.UserType == "Reviewer")
                            {
                                return RedirectToAction("Index", "Reviewer");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Incorrect Role";
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Incorrect Password/UserName. Please try again or register for an account below!";
                            return View();
                        }
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

        public List<Project> PopulateAdminDashboard()
        {
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
                    project.Comments = row["Comment"].ToString();

                    int status = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 2)
                    {
                        project.ProjectStatus = "Pending";
                        theProjects.Add(project);
                    }
                }
            }
            return theProjects;
        }


    }
}
