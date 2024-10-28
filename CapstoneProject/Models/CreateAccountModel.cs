using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
    public class CreateAccountModel
    {
        private string firstName;
        private string lastName;
        private string email;
        private string organization;

        public CreateAccountModel()
        {
        }

        public CreateAccountModel(string firstName, string lastName, string email, string organization)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Organization = organization;
        }

        [Required(ErrorMessage = "*First Name is required*")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters long")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Required(ErrorMessage = "*Last Name is required*")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters long")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [Required(ErrorMessage = "*Email is required*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [StringLength(100, ErrorMessage = "Organization name cannot be longer than 100 characters")]
        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }
    }
}
