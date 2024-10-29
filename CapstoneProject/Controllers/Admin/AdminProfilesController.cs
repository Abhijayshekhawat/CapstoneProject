﻿using CapstoneProject.Models.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static CapstoneProject.Models.AdminModel;
using System.Diagnostics;
using Profile = CapstoneProject.Models.ClassLibrary.Profile;
//should be used for admin to manage all user profiles
namespace CapstoneProject.Controllers.Admin
{
    public class AdminProfilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageProfiles()
        {
            //Use stored procedure to get profile data from datatable
            //create profile objects and add them to the viewbag

            Profile p = new Profile();
            DataSet ds = new DataSet();

            ds = p.GetProfiles();

            List<Profile> profiles = new List<Profile>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) //if the dataset is not null or empty
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                   Profile profile = new Profile();
                   profile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                   profile.FirstName = row["FirstName"].ToString();
                   profile.LastName = row["LastName"].ToString();
                   profile.Organization = row["Organization"].ToString();
                   profile.Email = row["Email"].ToString();

                   if (row["LastUpdatedStatus"].Equals(DBNull.Value))
                   {
                        profile.Status = "NULL Value";
                        profiles.Add(profile);
                   }
                   else
                    {
                        int status = Convert.ToInt32(row["LastUpdatedStatus"]);

                        if (status == 1)
                        {
                            profile.Status = "Approved";
                        }
                        else if (status == 2)
                        {
                            profile.Status = "Pending";
                        }
                        else
                        {
                            profile.Status = "Rejected";
                        }
                        profiles.Add(profile);
                    }
                }
                   
            }

            ViewBag.AdminViewProfiles = profiles; //viewbag containing the profiles

            return View("~/Views/Admin/AdminManageProfiles.cshtml", profiles);
        }

        public IActionResult ViewAProfile(int ProfileID)
        {
            Debug.WriteLine("The profileID for this profile is " + ProfileID); //testing to see that the profileID and projectID is being passed to the view

            ViewedProfile p = new ViewedProfile();
            DataSet ds = new DataSet();

            ds = p.GetViewedProfileDetails(ProfileID);

            ViewedProfile viewedProfile = new ViewedProfile();
            List<ViewedProfile> theProfile = new List<ViewedProfile>();

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows) //each record in the ds
                {
                    viewedProfile.ProfileID = Convert.ToInt32(row["ProfileID"]);
                    viewedProfile.FirstName = row["FirstName"].ToString();
                    viewedProfile.LastName = row["LastName"].ToString();
                    viewedProfile.Organization = row["Organization"].ToString();
                    viewedProfile.Email = row["Email"].ToString();
                    viewedProfile.SubmissionDate = Convert.ToDateTime(row["SubmissionDate"]).ToString("MM/dd/yyyy");
                    viewedProfile.StatusChangeDateTime = Convert.ToDateTime(row["StatusChangeDateTime"]).ToString("MM/dd/yyyy");
                    viewedProfile.Comment = row["Comment"].ToString();

                    int status = Convert.ToInt32(row["LastUpdatedStatus"]); //project status is stored as an int in db, for the admin view we want to show the string
                    if (status == 1)
                    {
                        viewedProfile.Status = "Approved";
                    }
                    else if (status == 2)
                    {
                        viewedProfile.Status = "Pending";
                    }
                    else
                    {
                        viewedProfile.Status = "Rejected";
                    }

                    theProfile.Add(viewedProfile);
                }
                ViewBag.AdminViewedProfile = theProfile;
            }

            return View("~/Views/Admin/AdminViewProfile.cshtml",viewedProfile);
        }
    }
}
