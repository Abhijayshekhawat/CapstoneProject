﻿using CapstoneProject.Models;
using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CapstoneProject.Controllers
{
    public class UserDashController : Controller
    {
        // This method returns the UserDashboard view
        public IActionResult UserDashboard()
        {//Use stored procedure to get project data from datatable
         //create project objects and add them to the viewbag

            AdminModel p = new AdminModel();
            Profile profile = new Profile();
            //dataset with all profiles and projects
            DataSet profileDs = profile.GetProfiles();
            DataSet projectDs = p.GetProjects();
            //list of project objects and profiles
            List<Project> theProjects = new List<Project>(); 
            List<Profile> profiles = new List<Profile>();

            if (projectDs.Tables.Count > 0 && projectDs.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in projectDs.Tables[0].Rows) //each record in the ds
                {
                    Project project = new Project(); //create a project object for each record
                    project.ProjectID = Convert.ToInt32(row["ProjectID"]);
                    project.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    project.ProjectName = row["ProjectName"].ToString();
                    project.ShortDesc = row["ProjectDescription"].ToString();

                    int status = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 2)
                    {
                        project.ProjectStatus = "Pending";
                        project.Comments = row["Comment"].ToString();
                        theProjects.Add(project);
                    }
                   
                }
            }

            if (profileDs.Tables.Count > 0 && profileDs.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in profileDs.Tables[0].Rows) //each record in the ds
                {
                    Profile userProfile = new Profile();
                    userProfile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    userProfile.FirstName = row["FirstName"].ToString();
                    userProfile.LastName = row["LastName"].ToString();
                    userProfile.Organization = row["Organization"].ToString();
                    userProfile.Email = row["Email"].ToString();

                    if (row["LastUpdatedStatus"].Equals(DBNull.Value))
                    {
                        userProfile.Status = "NULL Value";
                        profiles.Add(userProfile);
                    }
                    else
                    {
                        int status = Convert.ToInt32(row["LastUpdatedStatus"]);

                        if (status == 2)
                        {
                            userProfile.Status = "Pending";
                            profiles.Add(userProfile);
                        }
                    }
                }

            }

            ViewBag.AdminViewProfiles = profiles; //viewbag containing the profiles

            ViewBag.AdminViewProjects = theProjects; //viewbag containing the list of projects


            // Specify the path to the view inside the "Dashboard" folder
            return View("~/Views/Dashboard/UserDashboard.cshtml");
        }

        public IActionResult CreateNewProject()
        
        { 
            return View("~/Views/Account/CreateNewProject.cshtml");
        
        
        }

    }
}



//app.MapControllerRoute(
  //  name: "default",
    //pattern: "{controller=UserDash}/{action=UserDashboard}/{id?}");