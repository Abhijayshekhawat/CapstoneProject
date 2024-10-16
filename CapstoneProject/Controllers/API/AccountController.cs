using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;


namespace CapstoneProject.Controllers.API
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login")]
        public string Login([FromBody] User user)
        {
            string result;
            string userType = user.CheckUserType(user.Email);
            int Login = user.Login(user.Email, user.PasswordHash);

            if (Login > 0 && userType != null)
            {
                result = userType;
            }
            else { result = ""; }
            return result;
        }
    }
}
