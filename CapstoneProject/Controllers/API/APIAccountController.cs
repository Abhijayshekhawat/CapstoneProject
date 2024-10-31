using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;


namespace CapstoneProject.Controllers.API
{
    [Route("api/Account")]
    [ApiController]
    public class APIAccountController : ControllerBase
    {
        [HttpPost("Login")]
        public Profile Login([FromBody] User user)
        {
            Profile profile = user.Login(user.Email, user.PasswordHash);

            return profile ?? null; // Return the profile if it exists; otherwise, return null
        }

        [HttpPost("CreateAccount")]
        public string CreateAccount([FromBody] Profile profile)
        {
            try
            {
                int profileId;

                // Create a profile and get the ProfileID
                int success = profile.CreateProfile(
                    profile.Organization,
                    profile.FirstName,
                    profile.LastName,
                    profile.Email,
                    profile.SubmissionDate,
                    out profileId
                );

                if (success > 0)
                {
                    // Add initial profile status as Pending
                    profile.AddProfileStatus(profileId, DateTime.Now, "Profile created with pending status.");
                    return "Created";
                }
                else
                {
                    return "Exists";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}
