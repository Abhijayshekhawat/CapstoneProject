using CapstoneProject.Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapstoneProject.Models.AdminModel;

namespace CapstoneProject.Models.ClassLibrary
{
    //  CommentID INT PRIMARY KEY,
    //ProfileID INT,
    //  CommentForProjectID INT,
    //  LastUpdatedStatus ENUM('Approved', 'Pending', 'Rejected') NOT NULL,
    //  StatusChangeDateTime VARCHAR(MAX),
    //  Comment VARCHAR(MAX),
    //  FOREIGN KEY(ProfileID) REFERENCES Profile(ProfileID),
    //  FOREIGN KEY(CommentForProjectID) REFERENCES NewProjects(ProjectID)
    public class Comment
    {
        private int commentid;
        private string commenterName;
        private int profileid;
        private int commentforprojectid;
        private string lastupdatedstatus;
        private string statuschangedatetime;
        private string description;
        private List<Comment> comments;

        public Comment() { }

        public Comment(int commentid, string commenterName, int profileid, int commentforprojectid, string lastupdatedstatus, string statuschangedatetime, string description)
        {
            this.commentid = commentid;
            this.commenterName = commenterName;
            this.profileid = profileid;
            this.commentforprojectid = commentforprojectid;
            this.lastupdatedstatus = lastupdatedstatus;
            this.statuschangedatetime = statuschangedatetime;
            this.description = description;



        }

        public int CommentID
        {
            get { return commentid; }
            set { commentid = value; }
        }
        public string CommenterName
        {
            get { return commenterName; }
            set { commenterName = value; }
        }
        public int ProfileID
        {
            get { return profileid; }
            set { profileid = value; }
        }

        public int CommentForProjectID
        {
            get { return commentforprojectid; }
            set { commentforprojectid = value; }
        }

        public string LastUpdatedStatus { get { return lastupdatedstatus; } set { lastupdatedstatus = value; } }

        public string StatusChangeDateTime { get { return statuschangedatetime; } set { statuschangedatetime = value; } }


        public string Description { get { return description; } set { description = value; } }

        public List<Comment> Comments { get { return comments; } set { comments = value; } }


        public DataSet GetProjectComments(int projectID)
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
                    CommandText = "GetProjectComments"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@ProjectID", projectID);
                objCommand.Parameters.Add(inputParameter);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }

        public DataSet GetProfileComments(int profileID)
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
                    CommandText = "GetProfileComments"
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
