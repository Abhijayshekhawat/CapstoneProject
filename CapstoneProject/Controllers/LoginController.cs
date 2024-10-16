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

namespace CapstoneProject.Controllers
{
    public class LoginController : Controller
    {
        string TestAPI_Url = "https://localhost:7151/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account"; //need to change this to the correct URL

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
            // Use LoginModel for validation purposes
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Manually extract form data for sending via JSON
            User user = new User
            {
                FirstName = "",
                LastName = "",
                UserType = "",
                Email = Request.Form["Email"].ToString(),  // Extract email from form
                PasswordHash = Request.Form["Password"].ToString()  // Extract password from form

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
                        string data = await response.Content.ReadAsStringAsync();

                        // Log the user type in the console
                        Console.WriteLine("User Type: " + data);

                        // Pass the user type to the view
                        ViewBag.UserType = data;

                        if (!string.IsNullOrEmpty(ViewBag.UserType))
                        {
                            return View("~/Views/Home/index.cshtml"); // Redirect to the homepage
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

            ViewBag.ErrorMessage = "Incorrect Password/UserName. Please try again!";
            return View();
        }


    }
}
