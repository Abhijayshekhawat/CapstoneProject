using CapstoneProject.Models.Utilities;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;

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
        private string usertype;

        public Profile() { }

        public Profile(int profileid, string organization, string firstname, string lastname, string email, DateTime submissiondate, string status, string usertype)
        {
            this.profileid = profileid;
            this.organization = organization;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.submissiondate = submissiondate;
            this.status = status;
            this.usertype = usertype;
        }

        public int ProfileID { get { return profileid; } set { profileid = value; } }

        public string Organization { get { return organization; } set { organization = value; } }

        public string FirstName { get { return firstname; } set { firstname = value; } }
        public string LastName { get { return lastname; } set { lastname = value; } }
        public string Email { get { return email; } set { email = value; } }
        public DateTime SubmissionDate { get { return submissiondate; } set { submissiondate = value; } }

        public string Status { get; set; }
        public string UserType { get; set; }
        public int CreateProfile(string organization, string firstName, string lastName, string email, DateTime submissionDate, out int profileId)
        {
            profileId = 0;
            using (Connection objDB = new Connection())
            {
                if (!objDB.Open())
                {
                    throw new Exception("Could not open database connection.");
                }

                SqlCommand objCommand = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "CreateProfile"
                };

                objCommand.Parameters.AddWithValue("@Organization", organization);
                objCommand.Parameters.AddWithValue("@FirstName", firstName);
                objCommand.Parameters.AddWithValue("@LastName", lastName);
                objCommand.Parameters.AddWithValue("@Email", email);
                objCommand.Parameters.AddWithValue("@SubmissionDate", submissionDate);
                objCommand.Parameters.Add("@ProfileID", SqlDbType.Int).Direction = ParameterDirection.Output;

                int rowsAffected = objDB.DoUpdateUsingCmdObj(objCommand);
                if (rowsAffected > 0)
                {
                    profileId = Convert.ToInt32(objCommand.Parameters["@ProfileID"].Value);
                }

                return rowsAffected;
            }
        }

        public void AddProfileStatus(int profileId, DateTime statusChangeDateTime, string comment)
        {
            using (Connection objDB = new Connection())
            {
                if (!objDB.Open())
                {
                    throw new Exception("Could not open database connection.");
                }

                SqlCommand objCommand = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "AddProfileStatus"
                };

                objCommand.Parameters.AddWithValue("@ProfileID", profileId);
                objCommand.Parameters.AddWithValue("@StatusChangeDateTime", statusChangeDateTime);
                objCommand.Parameters.AddWithValue("@Comment", comment);

                objDB.DoUpdateUsingCmdObj(objCommand);
            }
        }

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

