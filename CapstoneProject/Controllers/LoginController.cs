using Microsoft.AspNetCore.Mvc;
using CapstoneClassLibrary;
using CapstoneProject.Models;
using System.Data;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Controllers
{
    public class LoginController : Controller
    {
        string TestAPI_Url = "https://localhost:7277/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account";
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
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = new User();
            user.FirstName = "NoValue";
            user.LastName = "NoValue";
            user.Email = Request.Form["Email"].ToString();
            user.PasswordHash = EncryptionHelper.ComputeHash(Request.Form["Password"].ToString());
            user.UserType = "admin";
            user.IsActive = 1;
            //email
            string UserEmail = "";
            // for email
            string FirstName = "";
            string LastName = "";
            // Serialize an Account object into a JSON string.
            var jsonPayload = JsonSerializer.Serialize(user);
            try
            {
                // Send the account object to the Web API that will be used to store a new account record in the database.
                // Setup an HTTP POST Web Request and get the HTTP Web Response from the server.
                WebRequest request = WebRequest.Create(TestAPI_Url + "/Login");
                request.Method = "POST";
                request.ContentLength = jsonPayload.Length;
                request.ContentType = "application/json";
                // Write the JSON data to the Web Request
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonPayload);
                writer.Flush();
                writer.Close();
                // Read the data from the Web Response, which requires working with streams.
                WebResponse response = request.GetResponse();
                Stream theDataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(theDataStream);
                String data = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if (data == "true")
                {
                    return View("~/Views/Home/index.cshtml");
                } else {
                    ViewBag.ErrorMessage = "Incorrect Password/UserName. Please try again!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error: " + ex.Message;
            }
            return View();
        }
        
    }
}
