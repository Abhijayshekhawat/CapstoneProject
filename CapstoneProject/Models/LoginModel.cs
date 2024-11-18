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

        [Required(ErrorMessage = "An email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 50 characters.")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 50 characters.")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password format.")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
