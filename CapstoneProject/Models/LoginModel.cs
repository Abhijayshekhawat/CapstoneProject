using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
    public class LoginModel
    {
        private string email;
        private string password;

        public LoginModel()
        {
        }

        public LoginModel(string email, string pwd)
        {
            Email = email;
            Password = pwd;
        }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 50 characters long")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 50 characters long")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
