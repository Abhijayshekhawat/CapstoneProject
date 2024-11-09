using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneProject.Models.Utilities;

namespace CapstoneProject.Models.ClassLibrary
{
    // ProjectID INT PRIMARY KEY,
    //ProfileID INT,
    // ProjectDescription VARCHAR(MAX),
    // ProjectType CHAR(50),
    // SubmissionDate DATE,
    // ReviewDate DATE,
    // ReviewCode VARCHAR(20)
    public class NewProjects
    {
        private int projectid;
        private int profileid;
        private string projectdescription;
        private string projectname;
        private DateTime submissiondate;
        private DateTime reviewdate;
        private string reviewcode;







        public int CreateNewProject(int profileid, string projectdescription, string projectname)
        {

            // add new project
            Connection objDB = new Connection();

            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddNewProject";

            SqlParameter returnParameter = new SqlParameter("@ProjectID", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.Output;

            objCommand.Parameters.Add(returnParameter);





            SqlParameter inputParameter2 = new SqlParameter("@ProjectName", projectname);
            objCommand.Parameters.Add(inputParameter2);

            SqlParameter inputParameter3 = new SqlParameter("@ProjectDescription", projectdescription);
            objCommand.Parameters.Add(inputParameter3);

            SqlParameter inputParameter4 = new SqlParameter("@SubmissionDate", DateTime.Now.Date);
            objCommand.Parameters.Add(inputParameter4);


            SqlParameter inputParameter1 = new SqlParameter("@ProfileID", profileid);
            objCommand.Parameters.Add(inputParameter1);


            objDB.DoUpdateUsingCmdObj(objCommand);

            int projectid = int.Parse(returnParameter.Value.ToString());




            // add comment table
            Connection objDB2 = new Connection();

            SqlCommand objCommand2 = new SqlCommand();


            objCommand2.CommandType = CommandType.StoredProcedure;
            objCommand2.CommandText = "InsertComment";




            SqlParameter inputParameter11 = new SqlParameter("@CommentForProjectID", projectid);
            objCommand2.Parameters.Add(inputParameter11);

            string comment = "Pending until review by Reviewer or Admin";

            SqlParameter inputParameter12 = new SqlParameter("@Comment", comment);
            objCommand2.Parameters.Add(inputParameter12);

            SqlParameter inputParameter13 = new SqlParameter("@status", 2);
            objCommand2.Parameters.Add(inputParameter13);

            SqlParameter inputParameter14 = new SqlParameter("@StatusChangeDate", DateTime.Now);
            objCommand2.Parameters.Add(inputParameter14);

            objDB2.DoUpdateUsingCmdObj(objCommand2);

            // add project status table



            Connection objDB3 = new Connection();

            SqlCommand objCommand3 = new SqlCommand();

            objCommand3.CommandType = CommandType.StoredProcedure;
            objCommand3.CommandText = "InsertProjectStatus";



            SqlParameter inputParameter21 = new SqlParameter("@ProfileID", profileid);
            objCommand3.Parameters.Add(inputParameter21);
            SqlParameter inputParameter22 = new SqlParameter("@ProjectID", projectid);
            objCommand3.Parameters.Add(inputParameter22);


            SqlParameter inputParameter23 = new SqlParameter("@Comment", comment);
            objCommand3.Parameters.Add(inputParameter23);

            SqlParameter inputParameter24 = new SqlParameter("@StatusChangeDate", DateTime.Now);
            objCommand2.Parameters.Add(inputParameter24);

            objDB3.DoUpdateUsingCmdObj(objCommand3);

            return 1;



        }
        public List<NewProjects> GetNewProjectsByProjectID(int projectid)
        {

            List<NewProjects> ProjectList = new List<NewProjects>();

            Connection objDB = new Connection();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetClientUserProjectsByProjectID";
            SqlParameter inputParameter1 = new SqlParameter("@ProjectID", projectid);
            objCommand.Parameters.Add(inputParameter1);
            DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);



            DataTable dt2 = ds.Tables[0];
            NewProjects newProjects;

            foreach (DataRow dr in dt2.Rows)
            {

                DateTime Submission = Convert.ToDateTime(dr["SubmissionDate"]);
                newProjects = new NewProjects(profileid, dr["ProjectDescription"].ToString(), dr["ProjectName"].ToString(), Submission);


                ProjectList.Add(newProjects);

            }

            return ProjectList;
        }
        public List<NewProjects> GetNewProjects(int profileid)
        {

            List<NewProjects> ProjectList = new List<NewProjects>();

            Connection objDB = new Connection();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetClientUserProjects";
            SqlParameter inputParameter1 = new SqlParameter("@ProfileID", profileid);
            objCommand.Parameters.Add(inputParameter1);
            DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

            
            
            DataTable dt2 = ds.Tables[0];
            NewProjects newProjects;

            foreach (DataRow dr in dt2.Rows)
            {
                
                DateTime Submission = Convert.ToDateTime(dr["SubmissionDate"]);
                newProjects = new NewProjects(profileid, dr["ProjectDescription"].ToString(), dr["ProjectName"].ToString(), Submission);


                ProjectList.Add(newProjects);

            }

            return ProjectList;
        }

        public NewProjects() { }

        public NewProjects(int profileid, string projectdescription, string projectname, DateTime submissiondate)
        {

            this.profileid = profileid;
            this.projectdescription = projectdescription;
            this.projectname = projectname;
            this.submissiondate = submissiondate;




        }

        public int ProjectID { get { return projectid; } set { projectid = value; } }

        public int ProfileID { get { return profileid; } set { profileid = value; } }
        public string ProjectDescription { get { return projectdescription; } set { projectdescription = value; } }

        public string ProjectName { get { return projectname; } set { projectname = value; } }

        public DateTime Submissiondate { get { return submissiondate; } set { submissiondate = value; } }

        public DateTime Reviewdate { get { return submissiondate; } set { submissiondate = value; } }

        public string ReviewCode { get { return reviewcode; } set { reviewcode = value; } }



    }
}
