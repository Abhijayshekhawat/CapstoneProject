﻿using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models.ClassLibrary;
using CapstoneProject.Models;
using System.Data;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CapstoneProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        string TestAPI_Url = "https://localhost:7277/api/Account";
        string AccountAPI_Url = "https://cis-iis2.temple.edu/Spring2024/CIS3342_tuh18229/WebAPITest/api/Account";
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Submit(string email, string password)
        {
            // You can add logic here to validate the email and password if needed.
            // For now, it redirects to the Home page after login.
            return View("~/Views/Home/index.cshtml");
        }
        [HttpPost]
        public IActionResult Sso()
        {
            // This can redirect to a secure area after successful SSO login.
            return View("~/Views/Secure/index.cshtml");
        }
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch the connection string parts from appsettings and User Secrets
            string baseConnectionString = _configuration.GetConnectionString("Connection_Database");
            string passwordPart = _configuration["ConnectionStrings:Connection_Database_Password"]; // from secrets.json

            // Combine them to form the full connection string
            string fullConnectionString = $"{baseConnectionString}Password={passwordPart};";

            // Pass this connection string to the utilities project
            // Pass this connection string to the User class method
            User user = new User();
            int loginSuccess = user.Login(Request.Form["Email"].ToString(), Request.Form["Password"].ToString(), fullConnectionString);

            if (loginSuccess > 0)
            {
                return View("~/Views/Home/index.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Incorrect Password/UserName. Please try again!";
            }

            return View();
        }

    }
}
