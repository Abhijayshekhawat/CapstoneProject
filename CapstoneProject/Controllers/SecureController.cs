using CapstoneProject.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WS_LDAP_Search;

namespace CapstoneProject.Controllers
{
    public class SecureController : Controller
    {
        private readonly WebService _webService;

        // Inject WebService through constructor
        public SecureController(WebService webService)
        {
            _webService = webService;
        }

        public IActionResult Index()
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
