using CapstoneClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CapstoneProject.Models;
using Utilities;




//UserID INT PRIMARY KEY,
//   ProfileID INT,
//   Email CHAR(50) NOT NULL UNIQUE,
//   FirstName CHAR(50) NOT NULL,
//   LastName CHAR(50) NOT NULL UNIQUE,
//   PasswordHash VARCHAR(MAX) NOT NULL,
//   UserType ENUM('Client', 'Receiver', 'Admin') NOT NULL,
//   IsActive BIT NOT NULL,
//   FOREIGN KEY (ProfileID) REFERENCES Profile(ProfileID)

namespace CapstoneClassLibrary
{

    public class User
    {
        private int userid;
        private int profileid;
        private string email;
        private string firstname;
        private string lastname;
        private string passwordhash;
        private string usertype;
        private int isactive; 

        public User() { }

        public User(int userid, int profileid, string email, string firstname, string lastname, string passwordhash, string usertype, int isactive)
        {
            this.userid = userid;
            this.profileid = profileid;
            this.email = email;
            this.firstname = firstname;
            this.lastname = lastname;
            this.passwordhash = passwordhash;
            this.usertype = usertype;
            this.isactive = isactive;

        }

        
        public int UserID { get { return userid; } set {  userid = value; } }

        public int ProfileID { get { return profileid; } set {  profileid = value; } }

        public string Email { get { return email; } set { email = value; } }

        public string FirstName { get {  return firstname; } set {  firstname = value; } }

        public string LastName { get { return lastname; } set {  lastname = value; } }

        public string PasswordHash { get {  return passwordhash; } set {  passwordhash = value; } }

        public string UserType { get {  return usertype; } set { usertype = value;  } }

        public int IsActive { get {  return isactive; } set {  isactive = value; } }

        public int Login(string email, string password, string connectionString)
        {
            // Create an instance of the Connection class with the connection string
            using (Connection objDB = new Connection(connectionString))
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
                    CommandText = "UserLogin"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@Email", email);
                objCommand.Parameters.Add(inputParameter);

                SqlParameter inputParameter2 = new SqlParameter("@Password", password);
                objCommand.Parameters.Add(inputParameter2);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                // Check if there are any rows in the dataset to determine if the login was successful
                int SuccessfulLogin = ds.Tables[0].Rows.Count;

                // Return the result
                return SuccessfulLogin;
            }
        }

    }
}
