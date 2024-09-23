using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapstoneClassLibrary
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
    public class ProjectStatus {

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

        public int ProfileID { get { return profileid; } set {  profileid = value; } }
        public int ProjectID { get { return projectid; } set {  projectid = value; } }
        public string LastUpdatedStatus { get {  return lastupdatedstatus; } set {  lastupdatedstatus = value; } }

        public DateTime StatusChangeDateTime { get { return statuschangedatetime; } set { statuschangedatetime = value; } }
        public string Comment { get { return comment; } set { comment = value; } }

    }
}
