using Microsoft.AspNetCore.Mvc;
//should be used for clients to view their projects
namespace CapstoneProject.Controllers.Client
{
    public class ClientProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
