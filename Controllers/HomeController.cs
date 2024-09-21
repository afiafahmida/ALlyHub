using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Discovery;
using System.Web.UI;
using ALlyHub.Data;
using ALlyHub.Models;
using static System.Collections.Specialized.BitVector32;


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
            List<Project> projects = ProjectHelper.GrabProjects();
            return View(projects);
        }
        public ActionResult Login()
        {
            return View();
        }
        //User Authentication
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

                    Session["UserType"] = UserType;
                    if (UserType == "Developer")
                    {
                        Session["developerID"] = DatabaseHelper.GetDeveloperIdByUserId(userId);
                        Session["userID"] = userId;
                    }
                    else
                    {
                        Session["clientID"] = DatabaseHelper.GetClientIdByUserId(userId);
                        Session["userID"] = userId;
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
        //User Registration
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
                        // Send email notification
                        string subject = "You have successfully registered";
                        string body = "Congratulations! You have been successfully registered as a Client in our website. We hope to work with you in future.";
                        SendMessage.SendEmail(model.Email, subject, body);

                    }
                    else if (model.UserType == "Developer")
                    {
                        databaseHelper.RegisterDeveloper(userId);
                        // Send email notification
                        string subject = "You have successfully registered";
                        string body = "Congratulations! You have been successfully registered as a Developer in our website. We hope to work with you in future.";
                        SendMessage.SendEmail(model.Email, subject, body);

                    }
                    return RedirectToAction("Login", "Home");
                }
            }
            return View(model);
        }
        //Fetch User Data to Profile
        [HttpGet]
        public ActionResult Profile()
        {
            ProfileModel profileModel = new ProfileModel();
            ProfileModel experience = new ProfileModel();
            ProfileModel reviews = new ProfileModel();
            if (Session["userID"] == null)
            {
                return RedirectToAction("Login", "Home"); // Redirect to Login if user is not logged in
            }

            int userId = (int)Session["userID"];
            if (Session["userID"] != null && (string)Session["UserType"] == "Client")
            {
                int clientId = (int)Session["clientID"];
                profileModel = ProfileHelper.GetClientById(userId);
                profileModel.Projects = ProfileHelper.GetProjectsByClientId(clientId);
            }
            else if (Session["userID"] != null && (string)Session["UserType"] == "Developer")
            {
                int devId = (int)Session["developerID"];
                profileModel = ProfileHelper.GetProfileById(userId);
                profileModel.Projects = ProfileHelper.GetHandshakedProjectsByDeveloperId(devId);
                profileModel.Experiences = ProfileHelper.FetchExperience(userId);
            }
            reviews.Reviews = ProfileHelper.FetchReviews(userId);
            if (profileModel == null)
            {
                return HttpNotFound();
            }

            return View(profileModel);
        }
        //Update Profile
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
                        // Send email notification
                        string subject = "You have successfully Updated Your Profile";
                        string body = "Hey there User! You have sucessfully updated your profile.";
                        SendMessage.SendEmail(model.UserEmail, subject, body);

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
        public ActionResult DevProfile(ProfileModel model)
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
                    int rowsAffected = ProfileHelper.UpdateDeveloper(userId, model.DevDescription, model.AreaofExpertise, model.PortfolioLink, model.LinkedIn, model.Facebook);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction("Profile");
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
        public ActionResult AddWorkExperience(ProfileModel model)
        {
            string EndYEar = model.EndDateInput;
            int userId = (int)Session["userID"];
            if (Session["userID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.EndDateInput))
                {
                    EndYEar = "Present";
                }

                try
                {
                    int rowsAffected = ProfileHelper.AddWorkExperience(userId, model.CompanyInput, model.PositionInput, model.StartDateInput, EndYEar, model.JobDescriptionInput);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction("Profile");
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
            ProjectHelper projectHelper = new ProjectHelper();
            Project project = ProjectHelper.GetProjectById(projectId);
            bool hasbeenhandshaked = true;
            if (projectHelper.hasbeenhandshaked(projectId))
            {
                Session["HasBeenHandshaked"] = hasbeenhandshaked;
            }
            // Fetch the project details from the database based on projectId
            bool hasCompleted = true;
            Session["projectId"] = projectId;
            if (project == null)
            {
                return HttpNotFound(); // Handle if project with given ID is not found
            }
            if (projectHelper.hascompleted(projectId))
            {
                Session["HasCompleted"] = hasCompleted;
            }
            if (Session["userID"] != null && (string)Session["UserType"] == "Developer")
            {
                int userID = (int)Session["userID"];
                int devID = DatabaseHelper.GetDeveloperIdByUserId(userID);
                //ProjectHelper projectHelper = new ProjectHelper();
                bool hasApplied = true;
                bool devhasHandshaked = true;
                //bool hasbeenhandshaked = true;
               
                if (projectHelper.hasApplied(devID, projectId))
                {
                    Session["hasApplied"] = hasApplied;
                }
                if (projectHelper.devhasHandshaked(devID, projectId))
                {
                    Session["DevhasHandshaked"] = devhasHandshaked;
                }
              
              

            }
            if (Session["userID"] != null && (string)Session["UserType"] == "Client")
            {
                int userID = (int)Session["userID"];
                int clientID = DatabaseHelper.GetClientIdByUserId(userID);
               // ProjectHelper projectHelper = new ProjectHelper();

                bool clienthasHandshaked = true;
              
            
                if (projectHelper.clienthasHandshaked(clientID, projectId))
                {
                    Session["ClienthasHandshaked"] = clienthasHandshaked;
                }
             
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
                    ProjectHelper.InsertProject(project);


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
                int clientId = ApplicationHelper.GetClientIdByProject(project.ProjectID);
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
                ApplicationHelper.InsertApplicant(project);
                string subject = "You have successfully Applied";
                string body = "Hey there User! You have sucessfully applied for the post. We will let you know if you get accepted for the job";
                SendMessage.SendEmail(project.ApplicantsEmail, subject, body);
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
            var currentUserId = Session["clientID"];
            var project = ProjectHelper.GetProjectById(projectId); // Replace with your method to get the project

            if (project == null)
            {
                return HttpNotFound();
            }

            // Check if the logged-in developer is the one who posted the project
            if ((string)Session["UserType"] == "Client" && project.ClientID == (int)currentUserId)
            {
                var applicants = ApplicationHelper.GetApplicantsByProjectId(projectId); // Replace with your method to get the applicants
                return View(applicants);
            }
            else
            {
                // Optionally, you can show an unauthorized access message or redirect
                return RedirectToAction("Index", "Home");
            }
        }
        // Accept an applicant
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AcceptApplicant(Applicant applicant, string applicantEmail)
        {
            // Check if the session contains a valid UserID
            if (Session["UserID"] == null)
            {
                TempData["Error"] = "User is not logged in.";
                return RedirectToAction("Index", new { projectId = applicant.ProjectID });
            }

            // Safely cast session value to integer
            int clientid = (int)Session["clientID"];


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


            string projectName = ProjectHelper.GetProjectNameById(applicant.ProjectID);

            // Call the method in DatabaseHelper to insert the handshake record
            bool success = ApplicationHelper.InsertHandshake(handshake);
            string subject = "You have been accepted";
            string body = "Hey there User! You have been accepted for doing the job of " + projectName + ". The client will contact you very soon.";
            SendMessage.SendEmail(applicantEmail, subject, body);
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
        [HttpPost]
        public ActionResult SendOtp(ProfileModel profile)
        {
            if (!string.IsNullOrEmpty(profile.UserEmail))
            {
                if (DatabaseHelper.DoesEmailExist((string)profile.UserEmail))
                {
                    var random = new Random();
                    string randomNumber = string.Empty;

                    for (int i = 0; i < 8; i++)
                    {
                        // Generate a random digit between 0 and 9
                        int digit = random.Next(0, 10);
                        randomNumber += digit.ToString();
                    }

                    string subject = "Your Temporary OTP";
                    string body = "Here is your OTP to change Password " + (string)randomNumber;
                    SendMessage.SendEmail(profile.UserEmail, subject, body);
                    Session["TempOTP"] = (string)randomNumber;
                    Session["TempEmail"] = (string)profile.UserEmail;
                    return RedirectToAction("EnterOtp");
                }
            }
            ModelState.AddModelError("", "Please enter a valid email.");
            return View("ForgotPassword");
        }
        //OTP for Password Reset
        public ActionResult EnterOtp()
        {
            return View();
        }
        public ActionResult VerifyOtp(ProfileModel profile)
        {
            if ((string)profile.UserOtp == (string)Session["TempOTP"])
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                ModelState.AddModelError("", "Please enter correct OTP.");
                return RedirectToAction("ForgotPassword");
            }
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(ProfileModel profile)
        {
            if (profile.UserPassword != null)
            {
                string email = (string)Session["TempEmail"];
                DatabaseHelper.ChangePassword(email, (string)profile.UserPassword);
                Session.Clear();
                return RedirectToAction("Login");


            }
            else
            {
                ModelState.AddModelError("", "Please enter a password.");
                return RedirectToAction("ForgotPassword");
            }
        }
        SearchHelper searchHelper = new SearchHelper();
        public ActionResult Search(string queryText, string searchType)
        {
            Console.WriteLine("Search query: " + queryText);
            Console.WriteLine("Search type: " + searchType);

            if (string.IsNullOrWhiteSpace(queryText))
            {
                return RedirectToAction("Index");
            }


            searchType = searchType?.ToLower();


            if (searchType == "developers")
            {
                var developerResults = searchHelper.SearchUsers(queryText);
                if (developerResults == null || developerResults.Count == 0)
                {
                    return RedirectToAction("NoResult");
                }
                ViewBag.SearchUserResults = developerResults;
            }
            else if (searchType == "projects")
            {
                var projectResults = searchHelper.SearchProjects(queryText);
                if (projectResults == null || projectResults.Count == 0)
                {
                    return RedirectToAction("NoResult");
                }
                ViewBag.SearchProjectResults = projectResults;
            }

            return View("SearchResults");
        }
        public ActionResult SearchResults()
        {
            return View();
        }
        public ActionResult TalentDetailsFromSearch(int UserID)
        {
            int userid = UserID;
            int DeveloperID = DatabaseHelper.GetDeveloperIdByUserId(userid);
            FindTalentModel find = FindtalentHelper.FetchTalentByID((int)DeveloperID);
            if (find == null)
            {
                return HttpNotFound();
            }
            return View(find);
        }
        public ActionResult ClientDetailsFromSearch(int UserID)
        {
            int userid = UserID;
            int ClientID = DatabaseHelper.GetClientIdByUserId(userid);
            Search find = SearchHelper.FetchClientsByID((int)ClientID);
            if (find == null)
            {
                return HttpNotFound();
            }
            return View(find);
        }
        public ActionResult NoResult()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProjectSubmit(int projectId, HttpPostedFileBase uploadedFile)
        {
            try
            {
                // Check if a file is uploaded
                if (uploadedFile != null && uploadedFile.ContentLength > 0)
                {
                    // Get the file name
                    string fileName = Path.GetFileName(uploadedFile.FileName);

                 
                 

                    // Define the folder path where the file will be stored
                    string folderPath = Server.MapPath("~/UploadedFiles/");

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Define the file path where the file will be saved
                    string filePath = Path.Combine(folderPath, fileName);

                    // Save the file to the specified folder
                    uploadedFile.SaveAs(filePath);

                    // Retrieve user and project-related IDs
                    int userId = (int)Session["userID"];
                    int clientId = ApplicationHelper.GetClientIdByProject(projectId);
                    int devId = DatabaseHelper.GetDeveloperIdByUserId(userId);

                

                    // Use ProjectHelper to insert file information into the database
                    bool isInserted = ProjectHelper.InsertProjectFile(projectId, devId, clientId, fileName);

                    if (isInserted)
                    {
                        return RedirectToAction("ReviewByDev", new { projectId = projectId });
                    }
                    else
                    {
                        ViewBag.Message = "File uploaded, but database insertion failed.";
                        var project = ProjectHelper.GetProjectById(projectId);  // Load project details
                        return RedirectToAction("Index");  // Return to ProjectDetails view with project data
                    }
                }
                else
                {
                    ViewBag.Message = "Please upload a valid file.";
                    var project = ProjectHelper.GetProjectById(projectId);  // Load project details
                    return View("ProjectDetails", project);  // Return to ProjectDetails view with project data
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log them and display error message)
                ViewBag.Message = "Error: " + ex.Message;
                var project = ProjectHelper.GetProjectById(projectId);  // Load project details
                return View("ProjectDetails", project);  // Return to ProjectDetails view with project data
            }
        }
        public ActionResult ReviewByDev(int projectId)
        {
            var project = ProjectHelper.GetProjectById(projectId);
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReviewByDev(int projectid, Project project)
        {
            try
            {
                // Get ClientId by ProjectId
                int? clientId = ProjectHelper.GetClientIdByProject(projectid);
                if (!clientId.HasValue)
                {
                    ViewBag.Message = "Client not found for the given project.";
                    return RedirectToAction("ReviewByDev", new { projectid });
                }

                // Get UserId by ClientId
                int? userId = ProjectHelper.GetUserIdByClientId(clientId.Value);
                if (!userId.HasValue)
                {
                    ViewBag.Message = "User not found for the given client.";
                    return RedirectToAction("ReviewByDev", new { projectid });
                }

                // Get DeveloperId and ReviewerId
                int? devId = ProjectHelper.GetDevIdByProject(projectid);
                if (!devId.HasValue)
                {
                    ViewBag.Message = "Developer not found for the given project.";
                    return RedirectToAction("ReviewByDev", new { projectid });
                }

                int? reviewer = ProjectHelper.GetUserIdByDevId(devId.Value);
                if (!reviewer.HasValue)
                {
                    ViewBag.Message = "Reviewer not found.";
                    return RedirectToAction("ReviewByDev", new { projectid });
                }

                // Insert the review
                bool isInserted = ProjectHelper.InsertReview(userId.Value, reviewer.Value, project.DeveloperReview);

                if (isInserted)
                {
                    TempData["Message"] = "Review submitted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "There was an issue submitting the review.";
                    return RedirectToAction("ReviewByDev", new { projectid });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return RedirectToAction("ReviewByDev", new { projectid });
            }
        }
        public ActionResult DownloadFile(int projectId)
        {
            string fileName = ProjectHelper.GetFileNameByProjectId(projectId);

            if (string.IsNullOrEmpty(fileName))
            {
                ViewBag.Message = "File name not found for this project.";
                return View("Error");
            }

            // Define the file path
            string filePath = Server.MapPath("~/UploadedFiles/" + fileName);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                ViewBag.Message = "File not found.";
                return View("Error");  // Show an error view or message
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReviewByClient(int projectid, Project project)
        {
            try
            {
                // Get DeveloperId by ProjectId
                int? devId = ProjectHelper.GetDevIdByProject(projectid);
                if (!devId.HasValue)
                {
                    ViewBag.Message = "Developer not found for the given project.";
                    return RedirectToAction("ProjectDetails", new { projectid });
                }

                // Get ClientId by ProjectId
                int? clientId = ProjectHelper.GetClientIdByProject(projectid);
                if (!clientId.HasValue)
                {
                    ViewBag.Message = "Client not found for the given project.";
                    return RedirectToAction("ProjectDetails", new { projectid });
                }

                // Get UserId by ClientId (reviewer is the client in this case)
                int? reviewer = ProjectHelper.GetUserIdByClientId(clientId.Value);
                if (!reviewer.HasValue)
                {
                    ViewBag.Message = "User not found for the given client.";
                    return RedirectToAction("ProjectDetails", new { projectid });
                }

                // Get Developer's UserId
                int? userId = ProjectHelper.GetUserIdByDevId(devId.Value);
                if (!userId.HasValue)
                {
                    ViewBag.Message = "User not found for the given developer.";
                    return RedirectToAction("ProjectDetails", new { projectid });
                }

                // Insert the client review (note that userId is the developer's UserId, reviewer is the client)
                bool isInserted = ProjectHelper.InsertReview(userId.Value, reviewer.Value, project.ClientReview);

                if (isInserted)
                {
                    TempData["Message"] = "Review submitted successfully!";
                    return RedirectToAction("Index", new { projectid });
                }
                else
                {
                    ViewBag.Message = "There was an issue submitting the review.";
                    return RedirectToAction("ProjectDetails", new { projectid });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return RedirectToAction("ProjectDetails", new { projectid });
            }
        }

    }
}
    