using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneProject.Models.Utilities;
using static CapstoneProject.Models.AdminModel;




//UserID INT PRIMARY KEY,
//   ProfileID INT,
//   Email CHAR(50) NOT NULL UNIQUE,
//   FirstName CHAR(50) NOT NULL,
//   LastName CHAR(50) NOT NULL UNIQUE,
//   PasswordHash VARCHAR(MAX) NOT NULL,
//   UserType ENUM('Client', 'Receiver', 'Admin') NOT NULL,
//   IsActive BIT NOT NULL,
//   FOREIGN KEY (ProfileID) REFERENCES Profile(ProfileID)

namespace CapstoneProject.Models.ClassLibrary
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


        public int UserID { get { return userid; } set { userid = value; } }

        public int ProfileID { get { return profileid; } set { profileid = value; } }

        public string Email { get { return email; } set { email = value; } }

        public string FirstName { get { return firstname; } set { firstname = value; } }

        public string LastName { get { return lastname; } set { lastname = value; } }

        public string PasswordHash { get { return passwordhash; } set { passwordhash = value; } }

        public string UserType { get { return usertype; } set { usertype = value; } }

        public int IsActive { get { return isactive; } set { isactive = value; } }

        public Profile Login(string email, string password)
        {
            // Create an instance of the Connection class with the connection string
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
                    CommandText = "GetUserProfile"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@Email", email);
                objCommand.Parameters.Add(inputParameter);

                SqlParameter inputParameter2 = new SqlParameter("@Password", password);
                objCommand.Parameters.Add(inputParameter2);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                // Check if there are any rows in the dataset to determine if the login was successful

                Profile profile = new Profile();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (bool.Parse(row["IsActive"].ToString()) == true)
                    {
                        // Assign values to the User object's properties from the first row
                        profile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                        profile.Organization = row["Organization"].ToString();
                        profile.FirstName = row["FirstName"].ToString();
                        profile.LastName = row["LastName"].ToString();
                        profile.Email = row["Email"].ToString();
                        profile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]);
                        profile.UserType = row["UserTypeName"].ToString();
                        return profile;
                    }
                    else
                    {
                        profile.FirstName = "Inactive";
                    }
                }
                // Return the result
                return profile;
            }
        }

        public Profile SSOLogin(string email)
        {
            // Create an instance of the Connection class with the connection string
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
                    CommandText = "GetUserProfileByEmail"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@Email", email);
                objCommand.Parameters.Add(inputParameter);

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                // Check if there are any rows in the dataset to determine if the login was successful

                Profile profile = new Profile();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (bool.Parse(row["IsActive"].ToString()) == true)
                    {
                        // Assign values to the User object's properties from the first row
                        profile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                        profile.Organization = row["Organization"].ToString();
                        profile.FirstName = row["FirstName"].ToString();
                        profile.LastName = row["LastName"].ToString();
                        profile.Email = row["Email"].ToString();
                        profile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]);
                        profile.UserType = row["UserTypeName"].ToString();
                        return profile;
                    }
                    else
                    {
                        profile.FirstName = "Inactive";
                    }
                }
                // Return the result
                return profile;
            }
        }

        public string CheckUserType(string email)
        {
            // Create an instance of the Connection class with the connection string
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
                    CommandText = "GetUserTypeByEmail"
                };

                // Add parameters to the command
                SqlParameter inputParameter = new SqlParameter("@Email", email);
                objCommand.Parameters.Add(inputParameter);

                SqlParameter returnParameter = new SqlParameter("@UserTypeName", SqlDbType.VarChar, 50);
                returnParameter.Direction = ParameterDirection.Output;
                objCommand.Parameters.Add(returnParameter);
                // Use the Connection class's method to execute
                objDB.DoUpdateUsingCmdObj(objCommand); // This will execute the stored procedure

                // Retrieve the value of the output parameter
                string userTypeName = returnParameter.Value.ToString();

                // Return the UserTypeName or a message if not found
                return string.IsNullOrEmpty(userTypeName) ? "None" : userTypeName;
            }
        }

        public DataSet GetProjectsById()
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
                    CommandText = "GetProjectsByUser",
                };

                //objCommand.Parameters.AddWithValue("@ProfileId", INTEGER REPRESENTING PROFILE ID);
                objCommand.Parameters.AddWithValue("@ProfileId", 1);
                //stored procedure needs a @ProfileId Integer as parameter
                //NOTE we do not have a way of getting user id in our stored procedures yet
                //we need to return userid when they sucesfully login.. 1 is used as a testing number above

                // Use the Connection class's method to execute the SqlCommand and get a DataSet
                DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

                return ds;
            }
        }

        public int ChangeUserType(int ProfileID, int UserType)
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
                    CommandText = "AdminChangeUserType"
                };

                objCommand.Parameters.AddWithValue("@ProfileID", ProfileID);
                objCommand.Parameters.AddWithValue("@UserType", UserType);

                //returns rows affected
                return objDB.DoUpdateUsingCmdObj(objCommand);

            }
        }

        public bool CreateUser(int profileID, string email, string firstName, string lastName, int userType, bool isActive, out string password)
        {
            // Create an instance of the Connection class with the connection string
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
                    CommandText = "CreateUser" // The name of your stored procedure
                };
                password = passwordGenerator();
                // Add parameters to the command
                objCommand.Parameters.Add(new SqlParameter("@ProfileID", profileID));
                objCommand.Parameters.Add(new SqlParameter("@Email", email));
                objCommand.Parameters.Add(new SqlParameter("@FirstName", firstName));
                objCommand.Parameters.Add(new SqlParameter("@LastName", lastName));
                objCommand.Parameters.Add(new SqlParameter("@Password", EncryptionHelper.ComputeHash(password))); // Ensure password is hashed before passing
                objCommand.Parameters.Add(new SqlParameter("@UserType", userType));
                objCommand.Parameters.Add(new SqlParameter("@IsActive", isActive));

                try
                {
                    // Execute the command
                    objDB.DoUpdateUsingCmdObj(objCommand); // Assuming DoUpdateUsingCmdObj executes non-query commands
                    return true; // Return true if the execution is successful
                }
                catch (SqlException ex)
                {
                    // Handle SQL exceptions, e.g., duplicate email or foreign key violations
                    throw new Exception($"Database error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        private string passwordGenerator()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=<>?{}[]|";
            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(chars.Length);
                result.Append(chars[index]);
            }

            return result.ToString();

        }
    }
}
