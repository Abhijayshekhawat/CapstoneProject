using CapstoneProject.Models.Utilities;
using System.Data.SqlClient;
using System.Data;

namespace CapstoneProject.Models.ClassLibrary
{
    public class Profile
    {
        

        private int profileid;
        private string organization;
        private string firstname;
        private string lastname;
        private string email;
        private DateTime submissiondate;
        private string status;

        public Profile() { }

        public Profile(int profileid, string organization, string firstname, string lastname, string email, DateTime submissiondate, string status)
        {
            this.profileid = profileid;
            this.organization = organization;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.submissiondate = submissiondate;
            this.status = status;
        }

        public int ProfileID { get { return profileid; } set { profileid = value; } }

        public string Organization { get { return organization; } set { organization = value; } }

        public string FirstName { get { return firstname; } set { firstname = value; } }
        public string LastName { get { return lastname; } set { lastname = value; } }
        public string Email { get { return email; } set { email = value; } }
        public DateTime SubmissionDate { get { return submissiondate; } set { submissiondate = value; } }

        public string Status { get; set; }

        public DataSet GetProfiles()
        {
            using (Connection objDB = new Connection())
            {
                // Open the connection
                if (!objDB.Open())
                {
                    // Handle the case where the connection couldn't be opened
                    throw new Exception("Could not open database connection.");
                }

                // Create a SqlCommand object
                SqlCommand objCommand = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "AdminGetAllProfiles"
                };

                //no parameters to add, since its a Select * operation

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }
    }
}

