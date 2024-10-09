using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneProject.Models.ClassLibrary
{
    public class Project : PageModel
    {
        

        public Project()
        {

        }

        public Project(int projectID, int profileID, string projectName, string shortDesc, string projectStatus, string comments)
        {
            ProjectID = projectID;
            ProfileID = profileID;
            ProjectName = projectName;
            ShortDesc = shortDesc;
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

        public List<Project> projects
        {
            get;
            set;
        }

    }
}
