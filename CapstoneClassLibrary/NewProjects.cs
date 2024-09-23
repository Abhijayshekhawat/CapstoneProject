using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneClassLibrary
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
        private string projecttype;
        private DateTime submissiondate;
        private DateTime reviewdate;
        private string reviewcode;

        public NewProjects() { }

        public NewProjects(int projectid, int profileid, string projectdescription, string projecttype, DateTime submissiondate, DateTime reviewdate,string reviewcode) {
         
            this.projectid = projectid;
            this.profileid = profileid;
            this.projectdescription = projectdescription;
            this.projecttype = projecttype;
            this.submissiondate = submissiondate;
            this.reviewdate = reviewdate;
            this.reviewcode = reviewcode;

        
        
        }

        public int ProjectID { get { return projectid; } set { projectid = value; } }

        public int ProfileID { get { return profileid; } set {  profileid = value; } }
        public string ProjectdDescription { get {  return projectdescription; } set { projectdescription = value; } }

        public string ProjectType { get {  return projecttype; } set { projecttype = value; } }

        public DateTime Submissiondate { get {  return submissiondate; } set {  submissiondate = value; } }

        public DateTime Reviewdate { get { return submissiondate; } set { submissiondate = value; } }

        public string ReviewCode { get {  return reviewcode; } set {  reviewcode = value; } }



    }
}
