using CapstoneProject.Models.ClassLibrary;
using CapstoneProject.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using WS_LDAP_Search;
using CapstoneProject.Attributes;

namespace CapstoneProject.Controllers
{
    public class SecureController : Controller
    {
        string TestAPI_Url = "https://localhost:7151/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account";
        private readonly WebService _webService;

        // Inject WebService through constructor
        public SecureController(WebService webService)
        {
            _webService = webService;
        }
        public async Task<IActionResult> Index()
        {
            string accessnetID = ExtractAccessNetID(Request.Headers["remoteuser"]);
            TempleLDAPEntry userInfo = null;
            string errorMessage = null;

            if (!string.IsNullOrEmpty(accessnetID))
            {
                try
                {
                    userInfo = _webService.GetUserInfoByAccessNet(accessnetID);
                    if (userInfo == null)
                    {
                        errorMessage = accessnetID + "User not found or an error occurred.";
                    }
                    else
                    {
                        User user = new User
                        {
                            FirstName = "",
                            LastName = "",
                            UserType = "",
                            Email = accessnetID + "@temple.edu",  // Extract email from form
                            PasswordHash = ""  // Extract password from form
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
                                HttpResponseMessage response = await client.PostAsync(TestAPI_Url + "/SSOLogin", httpContent);

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

                }
                catch (Exception ex)
                {
                    errorMessage = $"Exception: {ex.Message}";
                    Console.WriteLine(ex);  // Logs the full exception to the console for debugging.
                }
            }
            else
            {
                errorMessage = "Invalid or missing AccessNet ID.";
            }

            ViewData["UserInfo"] = userInfo;
            ViewData["ErrorMessage"] = errorMessage;
            ViewData["Headers"] = GetAllHeaders();

            return View("~/Views/Secure/Index.cshtml");
        }

        private string ExtractAccessNetID(string remoteUserHeader)
        {
            if (string.IsNullOrEmpty(remoteUserHeader)) return null;
            int atIndex = remoteUserHeader.IndexOf('@');
            return atIndex > 0 ? remoteUserHeader.Substring(0, atIndex) : null;
        }

        private Dictionary<string, string> GetAllHeaders()
        {
            var headersDictionary = new Dictionary<string, string>();
            foreach (var header in Request.Headers)
            {
                headersDictionary.Add(header.Key, header.Value.ToString());
            }
            return headersDictionary;
        }
    }
}
