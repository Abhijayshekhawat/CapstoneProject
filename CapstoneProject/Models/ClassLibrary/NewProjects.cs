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



        public int CreateNewProject(string projectname, string projectdescription)
        {
            Connection objDB = new Connection();

            SqlCommand objCommand = new SqlCommand();

            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddNewProject";

            SqlParameter inputParameter2 = new SqlParameter("@ProjectName", projectname);
            objCommand.Parameters.Add(inputParameter2);

            SqlParameter inputParameter3 = new SqlParameter("@ProjectDescription", projectdescription);
            objCommand.Parameters.Add(inputParameter3);

            SqlParameter inputParameter4 = new SqlParameter("@SubmissionDate", DateTime.Now);
            objCommand.Parameters.Add(inputParameter4);


            int AddProject = objDB.DoUpdateUsingCmdObj(objCommand);

            return AddProject;
        }
        public List<NewProjects> GetNewProjects()
        {

            List<NewProjects> ProjectList = new List<NewProjects>();

            Connection objDB = new Connection();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AdminGetAllProjects";
            DataSet ds = objDB.GetDataSetUsingCmdObj(objCommand);

            DataTable dt2 = ds.Tables[0];
            NewProjects newProjects;

            foreach (DataRow dr in dt2.Rows)
            {
                DateTime Submission = Convert.ToDateTime(dr["SubmissionDate"]);
                newProjects = new NewProjects(dr["ProjectDescription"].ToString(), dr["ProjectName"].ToString(), Submission);


                ProjectList.Add(newProjects);

            }

            return ProjectList;
        }

        public NewProjects() { }

        public NewProjects(string projectdescription, string projectname, DateTime submissiondate)
        {


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
