using CapstoneProject.Models.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace CapstoneProject.Models.ClassLibrary
{
    public class AdminViewProject
    {
        string projectName;
        string projectDesc;
        string status;
        //string comments;
        string projectOwner;
        string email;
        string submissionDate;
        string lastReviewed;
        string reviewer;
        int projectID;
        public AdminViewProject()
        {

        }
        public int ProjectID
        {
            get; set;
        }
        public string ProjectName { get; set; }

        public string ProjectDesc { get; set; }

        public string Status { get; set; }

        //public string Comments { get; set; }

        public string ProjectOwner { get; set; }

        public string Email { get; set; }

        public string SubmissionDate { get; set; }

        public string LastReviewed { get; set; }

        public string Reviewer { get; set; }

        public List<Comment> Comment { get; set; }

        public List<AdminViewProject> theProject { get; set; }

        public DataSet GetViewedProjectDetails(int projectID)
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
                    CommandText = "AdminGetProjectData"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@ProjectID", projectID);
                objCommand.Parameters.Add(inputParameter);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }


    }
}
