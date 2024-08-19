using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
            if (find == null)
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
                bool isLoggedIn = DatabaseHelper.AuthenticateUser(model.Email, model.Password, out userId, out UserType );
                if (isLoggedIn)
                {
                    Session["userID"] = userId;
                    Session["UserType"] = UserType;
                    if(UserType == "Developer")
                    {
                        Session["developerID"] = DatabaseHelper.GetDeveloperIdByUserId(userId);
                    }
                    else
                    {
                        Session["clientID"] = DatabaseHelper.GetClientIdByUserId(userId);
                    }
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
            if (Session["userID"] != null && (string)Session["UserType"] == "Client")
            {
                int clientId = (int)Session["clientID"];
                profileModel = ProfileHelper.GetClientById(userId);
                profileModel.Projects = ProfileHelper.GetHandshakedProjectsByClientId(clientId);
            }
            else if (Session["userID"] != null && (string)Session["UserType"] == "Developer")
            {
                int devId = (int)Session["developerID"];
                profileModel = ProfileHelper.GetDeveloperById(userId);
                profileModel.Projects = ProfileHelper.GetHandshakedProjectsByDeveloperId(devId);
            }

            if (profileModel == null)
            {
                return HttpNotFound();
            }

            return View(profileModel);
        }
        [HttpPost]
        public ActionResult Profile(ProfileModel model, HttpPostedFileBase Photo)
        {
            string userId = Session["userID"].ToString();
            if (Session["userID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Photo != null && Photo.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Photo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Photo.SaveAs(path);
                        model.UserPhoto = fileName;
                    }

                    int rowsAffected = ProfileHelper.UpdateUser(
                        userId,
                        model.FirstName,
                        model.LastName,
                        model.UserEmail,
                        model.UserAddress,
                        model.UserPhone,
                        model.Country,
                        model.Languagee,
                        model.DOB,
                        model.UserPhoto
                    );

                    if (rowsAffected > 0)
                    {
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No records were updated. Please verify your input data.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                    // Optionally log the exception
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
                    int clientId = DatabaseHelper.GetClientIdByUserId(userId);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForJob(Project project, HttpPostedFileBase file)
        {
            // Validate user type
            if (Session["userID"] == null || (string)Session["UserType"] != "Developer")
            {
                System.Diagnostics.Debug.WriteLine("User is not authenticated or not a developer.");
                return RedirectToAction("Login", "Home");
            }

            try
            {
                int userId = (int)Session["userID"];
                int clientId = DatabaseHelper.GetClientIdByProject(project.ProjectID);
                int devId = DatabaseHelper.GetDeveloperIdByUserId(userId);

                // File handling
                string fileName = null;
                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/ApplicantFiles"), fileName);
                    file.SaveAs(path);
                }

                // Populate project object with applicant details
                project.ClientID = clientId;
                project.DeveloperID = devId;
                project.ApplicantsFile = fileName;

                // Database insertion
                DatabaseHelper.InsertApplicant(project);

                System.Diagnostics.Debug.WriteLine("Application successfully inserted.");
                return RedirectToAction("ProjectDetails", new { projectId = project.ProjectID });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in ApplyForJob: " + ex.Message);
                ModelState.AddModelError("", "There was an error submitting your application. Please try again later.");
                return RedirectToAction("ProjectDetails", new { projectId = project.ProjectID });
            }
        }


        public ActionResult ViewApplicants(int projectId)
        {
            // Assuming you have a method to get the current user's ID
            var currentUserId = Session["userID"];
            var project = DatabaseHelper.GetProjectById(projectId); // Replace with your method to get the project

            if (project == null)
            {
                return HttpNotFound();
            }

            // Check if the logged-in developer is the one who posted the project
            if ((string)Session["UserType"] == "Client" && project.ClientID == (int)currentUserId)
            {
                var applicants = DatabaseHelper.GetApplicantsByProjectId(projectId); // Replace with your method to get the applicants
                return View(applicants);
            }
            else
            {
                // Optionally, you can show an unauthorized access message or redirect
                return RedirectToAction("Index", "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AcceptApplicant(Applicant applicant)
        {
            // Check if the session contains a valid UserID
            if (Session["UserID"] == null)
            {
                TempData["Error"] = "User is not logged in.";
                return RedirectToAction("Index", new { projectId = applicant.ProjectID });
            }

            // Safely cast session value to integer
            int clientid;
            if (!int.TryParse(Session["UserID"].ToString(), out clientid))
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("ViewApplicants", new { projectId = applicant.ProjectID });
            }

            // Define the handshake model
            var handshake = new ALlyHub.Models.Handshake
            {
                ProjectID = applicant.ProjectID,
                DeveloperID = applicant.DeveloperID,
                ClientID = clientid,
                HandshakeDate = DateTime.Now,
                Status = "Accepted",
                Duration = "To be determined"
            };

            // Call the method in DatabaseHelper to insert the handshake record
            bool success = DatabaseHelper.InsertHandshake(handshake);

            if (success)
            {
                TempData["Message"] = "Applicant accepted successfully!";
            }
            else
            {
                TempData["Error"] = "An error occurred while accepting the applicant.";
            }

            // Redirect back to the ViewApplicants page
            return RedirectToAction("ViewApplicants", new { projectId = applicant.ProjectID });
        }

    }
}
