using CapstoneProject.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CapstoneProject.Models.AdminModel;

namespace CapstoneProject.Models.ClassLibrary
{

    //    CREATE TABLE ProjectStatus(
    //    StatusID INT PRIMARY KEY,
    //    ProfileID INT,
    //    ProjectID INT,
    //    LastUpdatedStatus ENUM('Approved', 'Pending', 'Rejected') NOT NULL,
    //    StatusChangeDateTime VARCHAR(MAX),
    //    Comment VARCHAR(MAX),
    //    FOREIGN KEY(ProfileID) REFERENCES Profile(ProfileID),
    //    FOREIGN KEY(ProjectID) REFERENCES NewProjects(ProjectID)
    //);
    public class ProjectStatus
    {

        private int statusid;
        private int profileid;
        private int projectid;
        private string lastupdatedstatus;
        private DateTime statuschangedatetime;
        private string comment;



        public ProjectStatus() { }

        public ProjectStatus(int statusid, int profileid, int projectid, string lastupdatedstatus, DateTime statuschangedatetime, string commment)
        {
            this.statusid = statusid;
            this.profileid = profileid;
            this.projectid = projectid;
            this.lastupdatedstatus = lastupdatedstatus;
            this.statuschangedatetime = statuschangedatetime;
            this.comment = commment;

        }

        public int StatusID { get { return statusid; } set { statusid = value; } }

        public int ProfileID { get { return profileid; } set { profileid = value; } }
        public int ProjectID { get { return projectid; } set { projectid = value; } }
        public string LastUpdatedStatus { get { return lastupdatedstatus; } set { lastupdatedstatus = value; } }

        public DateTime StatusChangeDateTime { get { return statuschangedatetime; } set { statuschangedatetime = value; } }
        public string Comment { get { return comment; } set { comment = value; } }

        public void UpdateProjectStatus(int profileID, int projectID, int status) //stored procedure to update project status in TB_NewProjects
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
                    CommandText = "AdminChangeProjectStatus"
                };

                SqlParameter inputParameter = new SqlParameter("@ReviewerID", profileID);
                objCommand.Parameters.Add(inputParameter);

                // Add parameters to the command
                SqlParameter inputParameter1 = new SqlParameter("@ProjectID", projectID);
                objCommand.Parameters.Add(inputParameter1);

                // Add parameters to the command
                SqlParameter inputParameter2 = new SqlParameter("@Status", status);
                objCommand.Parameters.Add(inputParameter2);

                objDB.DoUpdateUsingCmdObj(objCommand);
            }
        }
        public void DeleteRejectedProject(int projectID)
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
                    CommandText = "DeleteProjectByID"
                };



                // Add parameters to the command
                SqlParameter inputParameter1 = new SqlParameter("@ProjectID", projectID);
                objCommand.Parameters.Add(inputParameter1);

                // Add parameters to the command


                objDB.DoUpdateUsingCmdObj(objCommand);
            }

        }

        public void AddProjectComment(int commenterID, int projectID, string comment, int status) //Insert new comment into TB_Comment
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
                    CommandText = "AddProjectComment"
                };
                objCommand.Parameters.AddWithValue("@CommenterID", commenterID);
                objCommand.Parameters.AddWithValue("@ProjectID", projectID);
                objCommand.Parameters.AddWithValue("@Comment", comment);
                objCommand.Parameters.AddWithValue("@Status", status);

                objDB.DoUpdateUsingCmdObj(objCommand);
            }
        }

        public void AddCommentToProjectStatus(int projectID, string comment, int status) //Insert the same new comment into TB_ProjectStatus
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
                    CommandText = "AddCommentToProjectStatus"
                };

                objCommand.Parameters.AddWithValue("@ProjectID", projectID);
                objCommand.Parameters.AddWithValue("@Comment", comment);
                objCommand.Parameters.AddWithValue("@Status", status);

                objDB.DoUpdateUsingCmdObj(objCommand);

            }
        }
    }
}
