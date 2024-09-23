using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneClassLibrary
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
        private int profileid;
        private int commentforprojectid;
        private string lastupdatedstatus;
        private string statuschangedatetime;
        private string description ;


        public Comment() { }

        public Comment(int commentid, int profileid, int commentforprojectid, string lastupdatedstatus, string statuschangedatetime, string description)
        { 
            this.commentid = commentid; 
            this.profileid = profileid;
            this.commentforprojectid = commentforprojectid;
            this.lastupdatedstatus = lastupdatedstatus;
            this.statuschangedatetime = statuschangedatetime;
            this.description = description ;



        }

        public int CommentID
        {
            get { return commentid; }
            set { commentid = value; }
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

        public string LastUpdatedStatus { get { return lastupdatedstatus; } set {  lastupdatedstatus = value; } }

        public string StatusChangeDateTime { get {  return statuschangedatetime; } set {  statuschangedatetime = value; } }


        public string Description { get { return description; } set {  description = value; } }

    }
}
