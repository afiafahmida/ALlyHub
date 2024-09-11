using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ALlyHub.Models;
using System.Configuration;

namespace ALlyHub.Data
{
    public class DatabaseHelper
    {
        private static readonly string connectionString = ConnectDB.connect;

        public static int RegisterUser(string FirstName,string LastName, string email, string password, string address, string phone , string UserType)
        {
            int newUserId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FirstName, LastName, UserEmail, UserPassword, UserPhone, UserAddress , UserType) OUTPUT INSERTED.UserId VALUES (@FirstName, @LastName, @Email, @Password, @Phone, @Address ,@UserType)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@UserType", UserType);
                connection.Open();
                newUserId=(int)cmd.ExecuteScalar();
                connection.Close();
            }
            return newUserId;
        }
        public bool CheckUserExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE UserEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public void RegisterClient(int userID, string companyName, string clientLocation)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Client (UserId, CompanyName, ClientLocation) VALUES (@UserId, @CompanyName, @ClientLocation)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userID);
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@ClientLocation", clientLocation);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RegisterDeveloper(int userId)
        {
            using( SqlConnection connection = new SqlConnection( connectionString))
            {
                string query = "INSERT INTO Developer (UserId) VALUES (@userId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static bool AuthenticateUser(string email, string password , out int userId , out string UserType)
        {
            userId= 0;
            UserType = string.Empty;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Query to Check Credentials
                string query = "SELECT UserID,UserType FROM Users WHERE UserEmail = @email AND UserPassword = @password";
                SqlCommand command= new SqlCommand(query, connection);

                //Add Parameters to Prevent SQL Injection
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = reader.GetInt32(0);
                    UserType=reader.GetString(1);
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
        }
        public static List<Project> GrabProjects()
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT p.ProjectID, p.ProjectTitle, p.ShortDescription, p.PaymentAmount, p.ClientID, p.ExpertiseLevel, p.Duration, p.SkillSet, p.CompanyName , p.PostedOn
                FROM Project p
                JOIN Client c ON p.ClientID = c.ClientID ORDER BY PostedOn DESC";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project project = new Project
                        {
                            ProjectID = Convert.ToInt32(reader["ProjectID"]),
                            ProjectTitle = reader["ProjectTitle"].ToString(),
                            ShortDescription = reader["ShortDescription"].ToString(),
                            PaymentAmount = Convert.ToInt32(reader["PaymentAmount"]),
                            ClientID = Convert.ToInt32(reader["ClientID"]),
                            ExpertiseLevel = reader["ExpertiseLevel"].ToString(),
                            Duration = reader["Duration"].ToString(),
                            SkillSet = reader["SkillSet"].ToString(),
                            CompanyName = reader["CompanyName"].ToString(),
                            // New field
                           // PostedOn = reader["PostedOn"].ToString()
                        };

                        projects.Add(project);
                    }
                }

                connection.Close();
            }

            return projects;
        }
        public static string GetProjectNameById(int projectId)
        {
            string projectName = string.Empty;

            // Define the SQL query to select the project name by project ID
            string query = "SELECT ProjectTitle FROM Project WHERE ProjectID = @ProjectID";

            // Create a connection to the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Create a SqlCommand to execute the query
                SqlCommand cmd = new SqlCommand(query, conn);
                // Add the projectId parameter to the SqlCommand
                cmd.Parameters.AddWithValue("@ProjectID", projectId);

                try
                {
                    // Open the database connection
                    conn.Open();

                    // Execute the query and retrieve the project name
                    object result = cmd.ExecuteScalar();

                    // Check if result is not null, then convert it to string
                    if (result != null)
                    {
                        projectName = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the database operation
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Ensure the connection is always closed, even if an error occurs
                    conn.Close();
                }
            }

            // Return the project name
            return projectName;
        }

        public static Project GetProjectById(int projectId)
        {
            Project project = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT p.ProjectID, p.ProjectTitle, p.Description, p.PaymentAmount, p.ClientID, p.ExpertiseLevel, p.Duration, p.SkillSet, p.CompanyName , p.PostedOn , p.JobNature , c.ClientLocation , c.ClientDescription , c.Facebook , c.Linkedin
                FROM Project p
                INNER JOIN Client c ON p.ClientID = c.ClientID
                WHERE p.ProjectID = @ProjectID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProjectID", projectId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    project = new Project
                    {
                        ProjectID = Convert.ToInt32(reader["ProjectID"]),
                        ProjectTitle = reader["ProjectTitle"].ToString(),
                        Description = reader["Description"].ToString(),
                        PaymentAmount = Convert.ToInt32(reader["PaymentAmount"]),
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        ExpertiseLevel = reader["ExpertiseLevel"].ToString(),
                        Duration = reader["Duration"].ToString(),
                        SkillSet = reader["SkillSet"].ToString(),
                        CompanyName = reader["CompanyName"].ToString(),
                        JobNature = reader["JobNature"].ToString(),
                        ClientLocation = reader["ClientLocation"].ToString(),
                        ClientDescription = reader["ClientDescription"].ToString(),
                        // New field
                        // PostedOn = reader["PostedOn"].ToString()
                    };
                }

                connection.Close();
            }

            return project;
        }
        public static void InsertProject(Project project)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO Project 
            (ProjectTitle, ShortDescription, Description, PaymentAmount, ClientID, ExpertiseLevel, Duration, SkillSet, CompanyName) 
            VALUES 
            (@ProjectTitle, @ShortDescription, @Description, @PaymentAmount, @ClientID, @ExpertiseLevel, @Duration, @SkillSet, @CompanyName)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProjectTitle", project.ProjectTitle);
                command.Parameters.AddWithValue("@ShortDescription", project.ShortDescription);
                command.Parameters.AddWithValue("@Description", project.Description);
                command.Parameters.AddWithValue("@PaymentAmount", project.PaymentAmount);
                command.Parameters.AddWithValue("@ClientID", project.ClientID);
                command.Parameters.AddWithValue("@ExpertiseLevel", project.ExpertiseLevel);
                command.Parameters.AddWithValue("@Duration", project.Duration);
                command.Parameters.AddWithValue("@SkillSet", project.SkillSet);
                command.Parameters.AddWithValue("@CompanyName", project.CompanyName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static int GetClientIdByProject(int projectId)
        {
            int clientId = 0; // Initialize with a default value
            string query = "SELECT ClientID FROM Project WHERE ProjectID = @projectId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@projectId", projectId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        clientId = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Optional: Log that no result was found for the given projectId
                        System.Diagnostics.Debug.WriteLine($"GetClientIdByProject: No ClientID found for ProjectID: {projectId}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Optional: Log the exception for debugging purposes
                System.Diagnostics.Debug.WriteLine($"GetClientIdByProject: Error fetching ClientID for ProjectID {projectId}: {ex.Message}");
                // Consider rethrowing the exception or handling it as per your application's requirements
            }

            return clientId;
        }


        public static void InsertApplicant(Project applicant)
        {
            string query = @"
                INSERT INTO Applicants 
                (ProjectID, ClientID, DeveloperID, ApplicantsName, ApplicantsEmail, ApplicantsWebsite, ApplicantsFile, ApplicantsCoverLetter) 
                VALUES 
                (@ProjectID, @ClientID, @DeveloperID, @ApplicantsName, @ApplicantsEmail, @ApplicantsWebsite, @ApplicantsFile, @ApplicantsCoverLetter)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProjectID", applicant.ProjectID);
                command.Parameters.AddWithValue("@ClientID", applicant.ClientID);
                command.Parameters.AddWithValue("@DeveloperID", applicant.DeveloperID);
                command.Parameters.AddWithValue("@ApplicantsName", applicant.ApplicantsName);
                command.Parameters.AddWithValue("@ApplicantsEmail", applicant.ApplicantsEmail);
                command.Parameters.AddWithValue("@ApplicantsWebsite", applicant.ApplicantsWebsite );
                command.Parameters.AddWithValue("@ApplicantsFile", applicant.ApplicantsFile);
                command.Parameters.AddWithValue("@ApplicantsCoverLetter", applicant.ApplicantsCoverLetter);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No rows were inserted.");
                }
            }
        }



        public static int GetDeveloperIdByUserId(int userId)
        {
            int DevId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DeveloperID FROM Developer WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    DevId = Convert.ToInt32(result);
                }
            }
            return DevId;
        }

        public static int GetClientIdByUserId(int UserID)
        {
            int clientId = 0;
            string query = "SELECT ClientID FROM Client WHERE UserId = @UserId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    clientId = Convert.ToInt32(result);
                }
            }

            return clientId;
        }
        public static List<Applicant> GetApplicantsByProjectId(int projectId)
        {
            List<Applicant> applicants = new List<Applicant>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Applicants WHERE ProjectID = @ProjectID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProjectID", projectId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Applicant applicant = new Applicant
                    {
                        ApplicantID = Convert.ToInt32(reader["ApplicantID"]),
                        ProjectID = Convert.ToInt32(reader["ProjectID"]),
                        DeveloperID = Convert.ToInt32(reader["DeveloperID"]),
                        ApplicantsName = reader["ApplicantsName"].ToString(),
                        ApplicantsEmail = reader["ApplicantsEmail"].ToString(),
                        ApplicantsWebsite = reader["ApplicantsWebsite"].ToString(),
                        ApplicantsFile = reader["ApplicantsFile"].ToString(),
                        ApplicantsCoverLetter = reader["ApplicantsCoverLetter"].ToString(),
                    };

                    applicants.Add(applicant);
                }

                connection.Close();
            }

            return applicants;
        }
        public static bool InsertHandshake(Handshake handshake)
        {
            string query = "INSERT INTO Handshake (ProjectID,ClientID, DeveloperID, HandshakeDate, Status, Duration) " +
                           "VALUES (@ProjectID, @ClientID, @DeveloperID, @HandshakeDate, @Status, @Duration)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProjectID", handshake.ProjectID);
                cmd.Parameters.AddWithValue("@DeveloperID", handshake.DeveloperID);
                cmd.Parameters.AddWithValue("@ClientID", handshake.ClientID);
                cmd.Parameters.AddWithValue("@HandshakeDate", handshake.HandshakeDate);
                cmd.Parameters.AddWithValue("@Status", handshake.Status);
                cmd.Parameters.AddWithValue("@Duration", handshake.Duration);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                 
                }
                catch (Exception ex)
                {
                    // Log or handle the error
                    return false;
                }
            }
        }
        public static bool DoesEmailExist(string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE UserEmail = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public static void ChangePassword(string email, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "UPDATE Users SET UserPassword = @Password WHERE UserEmail = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", newPassword);
                connection.Open();
                command.ExecuteNonQuery();


            }
        }

    }

}
