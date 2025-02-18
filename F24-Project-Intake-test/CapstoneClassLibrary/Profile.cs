namespace CapstoneClassLibrary
{
    public class Profile
    {
        

        private int profileid;
        private string organization;
        private string firstname;
        private string lastname;
        private string email;
        private DateTime submissiondate;

        public Profile() { }

        public Profile(int profileid, string organization, string firstname, string lastname, string email, DateTime submissiondate)
        {
            this.profileid = profileid;
            this.organization = organization;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.submissiondate = submissiondate;

        }

        public int ProfileID { get { return profileid; } set { profileid = value; } }

        public string Organization { get { return organization; } set { organization = value; } }

        public string FirstName { get { return firstname; } set { firstname = value; } }
        public string LastName { get { return lastname; } set { lastname = value; } }
        public string Email { get { return email; } set { email = value; } }
        public DateTime SubmissionDate { get { return submissiondate; } set { submissiondate = value; } }

    }
}

