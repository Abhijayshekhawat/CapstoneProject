using CapstoneProject.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapstoneProject.Models.AdminModel;

namespace CapstoneProject.Models.ClassLibrary
{
	public class ProfileStatus
	{
		


		private int statusid;
		private int profileid;
		private string lastupdatedstatus; // can be enum for method ???

		private string statuschangeddatetime;
		private string comment;

        public ProfileStatus()
        { }


        public ProfileStatus(int statusid, int profileid, string lastupdatedstatus, string statuschangeddatetime, string comment)
		{
			this.statusid = statusid;
			this.profileid = profileid;
			this.lastupdatedstatus = lastupdatedstatus;
			this.statuschangeddatetime = statuschangeddatetime;
			this.comment = comment;


		}

		public void UpdateProfileStatus(int profileID, int status, string comment)
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
                    CommandText = "AdminChangeProfileStatus"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@ProfileID", profileID);
                objCommand.Parameters.Add(inputParameter);

                // Add parameters to the command
                SqlParameter inputParameter2 = new SqlParameter("@Status", status);
                objCommand.Parameters.Add(inputParameter2);

                // Add parameters to the command
                SqlParameter inputParameter3 = new SqlParameter("@Comment", comment);
                objCommand.Parameters.Add(inputParameter3);

                objDB.DoUpdateUsingCmdObj(objCommand);
            }
        }

		public void AddProfileComment(int profileID, int status, string comment)
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
                    CommandText = "AddProfileComment"
                };

                objCommand.Parameters.AddWithValue("@ProfileID", profileID);
                objCommand.Parameters.AddWithValue("@Comment", comment);
                objCommand.Parameters.AddWithValue("@Status", status);

                objDB.DoUpdateUsingCmdObj(objCommand);
            }
        }


	}
}
