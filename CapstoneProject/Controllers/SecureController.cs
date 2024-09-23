using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Controllers
{
    public class SecureController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Secure/Index.cshtml");
        }



        protected string GetShibbolethHeaderAttributes()
        {
            string employeeNumber = Request.Headers["employeeNumber"]; //Use this to retrieve the user's information via the web services  
            HttpContext.Session.SetString("SSO_Attribute_employeeNumber", employeeNumber);




            return (String.IsNullOrWhiteSpace(employeeNumber)) ? "N/A" : employeeNumber;
        }

        private bool IsLocalRequest()
        {
            return HttpContext.Connection.RemoteIpAddress.Equals(HttpContext.Connection.LocalIpAddress);
        }
    }
}
