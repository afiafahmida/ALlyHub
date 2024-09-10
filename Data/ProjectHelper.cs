using ALlyHub.Models;
using System;
using System.Collections.Generic;
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
        public static Project GetProjectById(int projectId)
        {
            Project project = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT p.ProjectID, p.ProjectTitle, p.Description, p.PaymentAmount, p.ClientID, p.ExpertiseLevel, p.Duration, p.SkillSet, c.CompanyName , p.PostedOn , p.JobNature , p.JobLocation , c.ClientLocation , c.ClientDescription , c.Facebook , c.Linkedin
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
                        JobLocation = reader["JobLocation"].ToString(),
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
        public static void InsertProject(Project project)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO Project 
            (ProjectTitle, ShortDescription, Description, PaymentAmount, ClientID, ExpertiseLevel, Duration, SkillSet, CompanyName , JobNature , JobLocation) 
            VALUES 
            (@ProjectTitle, @ShortDescription, @Description, @PaymentAmount, @ClientID, @ExpertiseLevel, @Duration, @SkillSet, @CompanyName ,@JobNature , @JobLocation)";

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
                command.Parameters.AddWithValue("@JobNature", project.JobNature);
                command.Parameters.AddWithValue("@JobLocation", project.JobLocation);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool hasApplied(int devID , int projectID)
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

    }
}