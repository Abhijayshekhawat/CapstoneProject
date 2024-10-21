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
            objCommand.CommandText = "TB_CreateNewProject_Temp";

            SqlParameter inputParameter2 = new SqlParameter("@ProjectName", projectname);
            objCommand.Parameters.Add(inputParameter2);

            SqlParameter inputParameter3 = new SqlParameter("@ProjectDescription", projectdescription);
            objCommand.Parameters.Add(inputParameter3);
           

            int AddProject = objDB.DoUpdateUsingCmdObj(objCommand);

            return AddProject;
        }

        public NewProjects() { }

        public NewProjects(int projectid, int profileid, string projectdescription, string projectname, DateTime submissiondate, DateTime reviewdate,string reviewcode) {
         
            this.projectid = projectid;
            this.profileid = profileid;
            this.projectdescription = projectdescription;
            this.projectname = projectname;
            this.submissiondate = submissiondate;
            this.reviewdate = reviewdate;
            this.reviewcode = reviewcode;

        
        
        }

        public int ProjectID { get { return projectid; } set { projectid = value; } }

        public int ProfileID { get { return profileid; } set {  profileid = value; } }
        public string ProjectDescription { get {  return projectdescription; } set { projectdescription = value; } }

        public string ProjectName { get {  return projectname; } set { projectname = value; } }

        public DateTime Submissiondate { get {  return submissiondate; } set {  submissiondate = value; } }

        public DateTime Reviewdate { get { return submissiondate; } set { submissiondate = value; } }

        public string ReviewCode { get {  return reviewcode; } set {  reviewcode = value; } }



    }
}
