using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
    public class AdminModel
    {
        public AdminModel() { }

        public class Project //parameters for projects that the admin views
        {
            public string ProjectName
            {
                get { return ProjectName; }
                set { ProjectName = value; }
            }

            public string ShortDesc
            {
                get { return ShortDesc; }
                set { ShortDesc = value; }
            }

            public string ProjectStatus
            {
                get { return ProjectStatus; }
                set { ProjectStatus = value; }
            }

            public string Comments
            {
                get { return Comments; }
                set { Comments = value; }
            }
        }

        public class Profile //parameters for the profiles the admin views
        {
            public int ProfileID
            {
                get { return ProfileID; }
                set { ProfileID = value; }
            }

            public string FirstName
            {
                get { return FirstName; }
                set { FirstName = value; }
            }

            public string LastName
            {
                get { return LastName; }
                set { LastName = value; }
            }

            public string Organization
            {
                get { return Organization; }
                set { Organization = value; }
            }

            public string Email
            {
                get { return Email; }
                set { Email = value; }
            }

            public string ReviewStatus
            {
                get { return ReviewStatus; }
                set { ReviewStatus = value; }
            }
        }
    }
}
