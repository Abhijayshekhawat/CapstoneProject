using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject.Models.ClassLibrary
{
	public class ProfileStatus
	{
		


		private int statusid;
		private int profileid;
		private string lastupdatedstatus; // can be enum for method ???

		private string statuschangeddatetime;
		private string comment;

        public ProfileStatus()
        { }


        public ProfileStatus(int statusid, int profileid, string lastupdatedstatus, string statuschangeddatetime, string comment)
		{
			this.statusid = statusid;
			this.profileid = profileid;
			this.lastupdatedstatus = lastupdatedstatus;
			this.statuschangeddatetime = statuschangeddatetime;
			this.comment = comment;


		}

		public void UpdateProfileStatus()
		{

		}

		public void AddCommentToProfileStatus()
		{

		}


	}
}
