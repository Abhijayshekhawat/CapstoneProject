using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;
using CapstoneProject.Models.Utilities;
using System.Runtime.CompilerServices;

namespace CapstoneProject.Models
{
    public class AdminModel
    {
        public AdminModel()
        {
        }

        public class Project //parameters for projects that the admin views
        {
            private int projectID;
            private int profileID;
            private string projectName;
            private string shortDesc;
            private string projectStatus;
            private string comments;

            public Project()
            {

            }

            public Project(int projectID, int profileID, string projectName, string shortDesc, string projectStatus, string comments)
            {
                ProjectID = projectID;
                ProfileID = profileID;
                ProjectName = projectName;
                ShortDesc = shortDesc;
                ProjectStatus = projectStatus;
                Comments = comments;
            }

            public int ProjectID
            {
                get;
                set;
            }

            public int ProfileID
            {
                get;
                set;
            }

            public string ProjectName
            {
                get;
                set;
            }

            public string ShortDesc
            {
                get;
                set;
            }

            public string ProjectStatus
            {
                get;
                set;
            }

            public string Comments
            {
                get;
                set;
            }
        }

        public class Profile //parameters for the profiles the admin views
        {
            public int ProfileID
            {
                get;
                set;
            }

            public string FirstName
            {
                get;
                set;
            }

            public string LastName
            {
                get;
                set;
            }

            public string Organization
            {
                get;
                set;
            }

            public string Email
            {
                get;
                set;
            }

            public string ReviewStatus
            {
                get;
                set;
            }
        }

        public DataSet GetProjects()
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
                    //CommandText = "AdminGetAllProjects"
                    CommandText = "DashboardGetPendingProjects"
                };

                //no parameters to add, since its a Select * operation

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }
    }
}
