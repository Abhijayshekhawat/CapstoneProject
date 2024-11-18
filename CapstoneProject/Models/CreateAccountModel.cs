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

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [Required(ErrorMessage = "An email address is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Required(ErrorMessage = "Please specify your organization.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Organization name must be between 2 and 100 characters.")]
        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }
    }
}
