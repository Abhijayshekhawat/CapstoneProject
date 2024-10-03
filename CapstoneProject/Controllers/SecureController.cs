using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CapstoneProject.Controllers
{
    public class SecureController : Controller
    {
        public IActionResult Index()
        {
            // Get all headers
            var headers = GetAllHeaders();

            // Pass headers to the view using ViewData
            ViewData["Headers"] = headers;

            return View("~/Views/Secure/Index.cshtml");
        }

        protected Dictionary<string, string> GetAllHeaders()
        {
            var headersDictionary = new Dictionary<string, string>();

            // Loop through all headers and add them to the dictionary
            foreach (var header in Request.Headers)
            {
                headersDictionary.Add(header.Key, header.Value.ToString());
            }

            return headersDictionary;
        }
    }
}
