using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneProject.Models.ClassLibrary
{
    public class Project : PageModel
    {


        public Project()
        {

        }

        public Project(int projectID, int profileID, string projectName, string shortDesc, string desc, string projectStatus, string comments)
        {
            ProjectID = projectID;
            ProfileID = profileID;
            ProjectName = projectName;
            ShortDesc = shortDesc;
            Desc = desc;
            ProjectStatus = projectStatus;
            Comments = comments;
        }

        public int ProjectID
        {
            get;
            set;
        }

        public int ProfileID
        {
            get;
            set;
        }

        public string ProjectName
        {
            get;
            set;
        }

        public string ShortDesc
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public string ProjectStatus
        {
            get;
            set;
        }

        public string Comments
        {
            get;
            set;
        }

        public string SubmittedBy
        {
            get;
            set;
        }

        public DateTime DateSubmitted
        {
            get;
            set;
        }
        public List<Project> projects
        {
            get;
            set;
        }

    }
}
