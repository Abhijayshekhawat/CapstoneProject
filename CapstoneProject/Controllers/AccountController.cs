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
            // Use LoginModel for validation purposes
            if (!ModelState.IsValid)
            {
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
                Status = "Pending"
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
                        return View(); // Redirect to the account submitted page
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

        public IActionResult CreateNewProject()
        {


            return View("~/Views/Account/CreateNewProject.cshtml");
        }

        public IActionResult AddNewProject()
        {

            NewProjects newProjects = new NewProjects();
            newProjects.ProjectName = Request.Form["ProjectName"].ToString();
            newProjects.ProjectDescription = Request.Form["ProjectDescription"].ToString();
            newProjects.CreateNewProject(newProjects.ProjectName, newProjects.ProjectDescription);

            return View("~/Views/Account/CreateNewProject.cshtml");
        }



    }
}
