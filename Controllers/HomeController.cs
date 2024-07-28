using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALlyHub.Data;
using ALlyHub.Models;


namespace ALlyHub.Controllers
{
    public class HomeController : Controller
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult FindTalent()
        {
            ViewBag.Message = "Find Talent";
            List<FindTalentModel> findTalents = FindtalentHelper.FetchTalents();
            return View(findTalents);
        }
        public ActionResult TalentDetails(int DeveloperID)
        {
            FindTalentModel find = FindtalentHelper.FetchTalentByID(DeveloperID);
            if(find== null)
            {
                return HttpNotFound();
            }
            return View(find);
        }

        public ActionResult FindJobs()
        {
            ViewBag.Message = "Find Jobs";
            List<Project> projects = DatabaseHelper.GrabProjects();
            return View(projects);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                int userId;
                string UserType;
                bool isLoggedIn = DatabaseHelper.AuthenticateUser(model.Email, model.Password, out userId, out UserType);
                if (isLoggedIn)
                {
                    Session["userID"] = userId;
                    Session["UserType"] = UserType;
                    return RedirectToAction("Profile", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (databaseHelper.CheckUserExists(model.Email))
                {
                    ModelState.AddModelError("", "User Already Exists");
                }
                else
                {
                    int userId = DatabaseHelper.RegisterUser(model.FirstName, model.LastName, model.Email, model.Password, model.Address, model.Phone, model.UserType);
                    if (model.UserType == "Client")
                    {
                        // Assuming you have companyName and clientLocation fields in your RegisterModel
                        databaseHelper.RegisterClient(userId, "", "");
                    }
                    else if (model.UserType == "Developer")
                    {
                        databaseHelper.RegisterDeveloper(userId);
                    }
                    return RedirectToAction("Login", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Profile()
        {
            ProfileModel profileModel = new ProfileModel();
            if (Session["userID"] == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to Login if user is not logged in
            }

            int userId = (int)Session["userID"];
            if((string)Session["UserType"]=="Client")
            {
                profileModel = ProfileHelper.GetClientById(userId);
            }
            else if((string)Session["UserType"] == "Developer")
            {
                profileModel = ProfileHelper.GetProfileById(userId);
            }

            if (profileModel == null)
            {
                return HttpNotFound();
            }

            return View(profileModel);
        }
        [HttpPost]
        public ActionResult Profile(ProfileModel model , HttpPostedFileBase Photo)
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to Login if user is not logged in
            }

            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);

                    // Optionally, save the path to the database
                    model.UserPhoto = fileName;
                }
                int rowsAffected = ProfileHelper.UpdateUser(model.UserId , model.FirstName , model.LastName , model.UserEmail , model.UserAddress , model.UserPhone , model.Country , model.Languagee ,model.DOB , model.UserPhoto);
                if(rowsAffected > 0)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    ModelState.AddModelError("","Error");
                }


            }
            return View(model);
        }
        public ActionResult Logout()
        {
            // Clear session on logout
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult ProjectDetails(int projectId)
        {
            // Fetch the project details from the database based on projectId
            Project project = DatabaseHelper.GetProjectById(projectId);

            if (project == null)
            {
                return HttpNotFound(); // Handle if project with given ID is not found
            }

            return View(project);
        }



        public ActionResult PostJobs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJobs(Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the UserID from the session
                    int userId = Convert.ToInt32(Session["userID"]);

                    // Fetch the ClientID using the UserID
                    int clientId = databaseHelper.GetClientIdByUserId(userId);

                    // Set the ClientID and PostedOn fields
                    project.ClientID = clientId;
                    project.PostedOn = DateTime.Now;

                    // Save the project to the database
                    DatabaseHelper.InsertProject(project);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(project);
        }
    }

}
