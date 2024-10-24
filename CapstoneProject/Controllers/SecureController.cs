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
            // Hardcoded AccessNet ID for testing
            string accessnetID = "tuh18229";

            // Retrieve user information from LDAP
            var userInfo = _webService.GetUserInfoByAccessNet(accessnetID);

            if (userInfo != null)
            {
                ViewData["UserInfo"] = userInfo;
            }
            else
            {
                Console.WriteLine("User not found or an error occurred.");
            }

            // Get all request headers
            var headers = GetAllHeaders();
            ViewData["Headers"] = headers;

            return View("~/Views/Secure/Index.cshtml");
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
