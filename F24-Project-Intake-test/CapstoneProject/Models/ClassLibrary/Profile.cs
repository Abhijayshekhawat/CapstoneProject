using CapstoneProject.Models.Utilities;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using static CapstoneProject.Models.AdminModel;

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
        private string isActive;

        public Profile() { }

        public Profile(int profileid, string organization, string firstname, string lastname, string email, DateTime submissiondate, string status, string usertype, string isActive)
        {
            this.profileid = profileid;
            this.organization = organization;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.submissiondate = submissiondate;
            this.status = status;
            this.usertype = usertype;
            this.isActive = isActive;
        }
        //reviewer details
        public Profile(string firstname, string lastname, string usertype)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.usertype = usertype;
        }

        public int ProfileID { get { return profileid; } set { profileid = value; } }

        public string Organization { get { return organization; } set { organization = value; } }

        public string FirstName { get { return firstname; } set { firstname = value; } }
        public string LastName { get { return lastname; } set { lastname = value; } }
        public string Email { get { return email; } set { email = value; } }
        public DateTime SubmissionDate { get { return submissiondate; } set { submissiondate = value; } }

        public string Status { get; set; }
        public string UserType { get { return usertype; } set { usertype = value; } }
        public string IsActive { get { return isActive; } set { isActive = value; } }
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

        public int UpdateProfile(Profile profile)
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
                    //need to create this stored procedure
                    CommandText = "UpdateProfile"
                };

                objCommand.Parameters.AddWithValue("@Organization", this.organization);
                objCommand.Parameters.AddWithValue("@FirstName", this.firstname);
                objCommand.Parameters.AddWithValue("@LastName", this.lastname);
                objCommand.Parameters.AddWithValue("@Email", this.email);
                objCommand.Parameters.AddWithValue("@ProfileID", this.profileid);
                int userTypeValue = this.usertype switch
                {
                    "Client" => 1,
                    "Reviewer" => 2,
                    "Admin" => 3,
                    _ => throw new ArgumentException("Invalid user type") // Handles unexpected values
                };
                bool isActiveValue = this.isActive switch
                {
                    "Active" => true,
                    "Inactive" => false,
                    _ => throw new ArgumentException("Invalid activation status") // Handles unexpected values
                };
                objCommand.Parameters.AddWithValue("@UserType", userTypeValue);
                objCommand.Parameters.AddWithValue("@IsActive", isActiveValue);

                //returns rows affected
                return objDB.DoUpdateUsingCmdObj(objCommand);

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
                return objDB.GetDataSetUsingCmdObj(objCommand);


            }
        }

        public Profile GetProjectReviewerDetails(int profileID)
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
                    CommandText = "GetProjectReviewerDetails"
                };

                objCommand.Parameters.AddWithValue("@ProfileID", profileID);


                // Execute the command and get the dataset
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                // Check if dataset has data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    // Map dataset values to the Profile object
                    Profile profile = new Profile
                    {
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        UserType = row["UserTypeName"]?.ToString()
                    };

                    return profile; // Return the populated Profile object
                }
                else
                {
                    return null; // Return null if no data is found
                }


            }
        }

        public Profile GetCommenterDetails(int profileID)
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
                    CommandText = "GetCommenterDetails"
                };

                objCommand.Parameters.AddWithValue("@ProfileID", profileID);


                // Execute the command and get the dataset
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                // Check if dataset has data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    // Map dataset values to the Profile object
                    Profile profile = new Profile
                    {
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        UserType = ""
                    };

                    return profile; // Return the populated Profile object
                }
                else
                {
                    return null; // Return null if no data is found
                }


            }
        }
    }
}

