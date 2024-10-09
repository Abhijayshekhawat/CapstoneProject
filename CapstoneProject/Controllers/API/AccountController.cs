using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CapstoneClassLibrary;

namespace CapstoneProject.Controllers.API
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login")]

        public bool Login([FromBody] User user)
        {
            return false;
            //bool result;
            //User privateinfo = user;

            //int Login = user.Login(user.Email, user.PasswordHash);

            //if (Login > 0)
            //{
            //    result = true;
            //}
            //else { result = false; }
            //return result;



        }
    }
}
