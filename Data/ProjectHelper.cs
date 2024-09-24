using ALlyHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ProjectHelper
    {
        private static readonly string connectionString = ConnectDB.connect;

        public static List<Project> GrabProjects()
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT p.ProjectID, p.ProjectTitle, p.ShortDescription, p.PaymentAmount, p.ClientID, p.ExpertiseLevel, p.Duration, p.SkillSet, c.CompanyName , p.PostedOn
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
                SELECT p.ProjectID, p.ProjectTitle, p.Description, p.PaymentAmount, p.ClientID, p.ExpertiseLevel, p.Duration, p.SkillSet, c.CompanyName , p.PostedOn , p.JobNature , c.ClientLocation , c.ClientDescription , c.Facebook , c.Linkedin
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
        public static int InsertProject(Project project)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Project 
                    (ProjectTitle, ShortDescription, Description, PaymentAmount, ClientID, ExpertiseLevel, Duration, SkillSet, JobNature,JobLocation) 
                    VALUES 
                    (@ProjectTitle, @ShortDescription, @Description, @PaymentAmount, @ClientID, @ExpertiseLevel, @Duration, @SkillSet,@JobNature,@ClientLocation)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProjectTitle", project.ProjectTitle);
                    command.Parameters.AddWithValue("@ShortDescription", project.ShortDescription);
                    command.Parameters.AddWithValue("@Description", project.Description);
                    command.Parameters.AddWithValue("@PaymentAmount", project.PaymentAmount);
                    command.Parameters.AddWithValue("@ClientID", project.ClientID);
                    command.Parameters.AddWithValue("@ExpertiseLevel", project.ExpertiseLevel);
                    command.Parameters.AddWithValue("@Duration", project.Duration);
                    command.Parameters.AddWithValue("@SkillSet", project.SkillSet);
                    command.Parameters.AddWithValue("@ClientLocation", project.ClientLocation);
                    command.Parameters.AddWithValue("@JobNature", project.JobNature);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Log or handle the exception
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine("Error: " + ex.Message);
            }
            return rowsAffected;

        }
        public bool hasApplied(int devID, int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from Applicants where DeveloperID=@devID and ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@devID", devID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool devhasHandshaked(int devID, int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from Handshake where DeveloperID=@devID and ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@devID", devID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool hasbeenhandshaked(int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from Handshake where ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@devID", devID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool clienthasHandshaked(int clientID, int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from Handshake where ClientID=@clientID and ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientID", clientID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool hascompletedclient(int clientID, int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from ProjectFile where ClientID=@clientID and ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientID", clientID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool hascompleted( int projectID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select COUNT(1) from ProjectFile where ProjectID=@projectID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                   // cmd.Parameters.AddWithValue("@devID", devID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public static bool InsertProjectFile(int projectID, int developerID, int clientID, string fileName)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ProjectFile (ProjectId, DeveloperID, ClientID, ProjectFileName) 
                         VALUES (@ProjectID, @DeveloperID, @ClientID, @ProjectFileName)";

                SqlCommand command = new SqlCommand(query, connection);

                // Use explicit data types instead of AddWithValue for better performance
                command.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.Int) { Value = projectID });
                command.Parameters.Add(new SqlParameter("@DeveloperID", SqlDbType.Int) { Value = developerID });
                command.Parameters.Add(new SqlParameter("@ClientID", SqlDbType.Int) { Value = clientID });
                command.Parameters.Add(new SqlParameter("@ProjectFileName", SqlDbType.VarChar, 100) { Value = fileName });

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Log or handle the exception
                    Console.WriteLine("SQL Error: " + ex.Message);
                    // Consider logging the error properly in production code.
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return rowsAffected > 0;
        }
        public static int GetClientIdByProject(int id)
        {
            int DevId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ClientID FROM Project WHERE ProjectID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", id);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    DevId = Convert.ToInt32(result);
                }
            }
            return DevId;
        }
        public static int GetDevIdByProject(int id)
        {
            int DevId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DeveloperID FROM Handshake WHERE ProjectID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", id);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    DevId = Convert.ToInt32(result);
                }
            }
            return DevId;
        }
        public static int GetUserIdByClientId(int ID)
        {
            int clientId = 0;
            string query = "SELECT UserID FROM Client WHERE ClientID = @UserId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", ID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    clientId = Convert.ToInt32(result);
                }
            }

            return clientId;
        }
        public static int GetUserIdByDevId(int ID)
        {
            int clientId = 0;
            string query = "SELECT UserID FROM Developer WHERE DeveloperID = @UserId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", ID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    clientId = Convert.ToInt32(result);
                }
            }

            return clientId;
        }
        public static bool InsertReview(int userId, int reviewerId, string reviewText)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Reviews (UserId, ReviewerId, ReviewText,CreatedAt)
                         VALUES (@UserId, @ReviewerId, @ReviewText,@CreatedAt)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Use SqlParameter for better performance and to avoid SQL injection
                    command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@ReviewerId", SqlDbType.Int) { Value = reviewerId });
                    command.Parameters.Add(new SqlParameter("@ReviewText", SqlDbType.NVarChar, -1) { Value = reviewText });
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery(); // Execute the query
                    }
                    catch (SqlException ex)
                    {
                        // Log SQL exception (consider using a logging framework here)
                        Console.WriteLine("SQL Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        // Log general exception
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return rowsAffected > 0; // Return true if one or more rows are affected
        }
        public static string GetFileNameByProjectId(int projectId)
        {
            string fileName = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProjectFileName FROM ProjectFile WHERE ProjectId = @ProjectID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.Int) { Value = projectId });

                        connection.Open();
                        var result = command.ExecuteScalar();

                        if (result != null && !string.IsNullOrWhiteSpace(result.ToString()))
                        {
                            fileName = result.ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL exception (use your preferred logging mechanism here)
                // For example: Log.Error("SQL Error: " + ex.Message);
                Console.WriteLine("SQL Error: " + ex.Message);
                return null; // Or handle the exception as per your application requirements
            }
            catch (Exception ex)
            {
                // Log general exception
                // For example: Log.Error("Error: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return null; // Or handle the exception as per your application requirements
            }

            return fileName;
        }


    }

}
