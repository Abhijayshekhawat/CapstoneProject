using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using static CapstoneProject.Models.AdminModel;

using System.Data.SqlClient;
using CapstoneProject.Models.Utilities;

namespace CapstoneProject.Models.ClassLibrary
{
    public class ViewedProfile : PageModel
    {
        int profileID;
        string firstName;
        string lastName;
        string organization;
        string email;
        string submissionDate;
        string status;
        string statusChangeDateTime;
        string comment;
        string usertype;
        string isactive;

        public int ProfileID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string SubmissionDate { get; set; }
        public string Status { get; set; }
        public string StatusChangeDateTime { get; set; }
        public string Comment { get; set; }

        public string UserType { get; set; }
        public string IsActive { get; set; }
        public string LastReviewed { get; set; }

        public string Reviewer { get; set; }
        public List<ViewedProfile> TheProfile { get; set; }

        public List<Comment> Comments { get; set; }

        public DataSet GetViewedProfileDetails(int profileID)
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
                    CommandText = "AdminGetProfileData"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@ProfileID", profileID);
                objCommand.Parameters.Add(inputParameter);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }
    }
}
