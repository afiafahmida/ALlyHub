using ALlyHub.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;

namespace ALlyHub.Data
{
    public class ProfileHelper 
    {
        private static readonly string connectionString = ConnectDB.connect;
        public static ProfileModel GetProfileById(int userId)
        {
            ProfileModel profile = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select u.FirstName , u.LastName, u.UserAddress , u.UserPhone , u.UserEmail, u.UserPhoto , d.DeveloperID, d.UserId ,d.DevDescription , " +
                    "d.AreaofExpertise , d.PortfolioLink , d.LinkedIn ,d.Facebook, u.Country , u.Languagee , u.DOB from Developer d " +
                    "JOIN Users u ON d.UserId = u.UserId where u.UserId=@UserId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    profile = new ProfileModel
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        UserAddress = reader["UserAddress"].ToString(),
                        UserPhone = reader["UserPhone"].ToString(),
                        UserEmail = reader["UserEmail"].ToString(),
                        DevDescription = reader["DevDescription"].ToString(),
                        AreaofExpertise = reader["AreaofExpertise"].ToString(),
                        PortfolioLink = reader["PortfolioLink"].ToString(),
                        LinkedIn = reader["LinkedIn"].ToString(),
                        UserPhoto = reader["UserPhoto"].ToString(),
                        Languagee = reader["Languagee"].ToString(),
                        Country= reader["Country"].ToString(),
                        DOB = reader["DOB"].ToString(),
                        Facebook = reader["Facebook"].ToString()
                    };
                }

                connection.Close();
            }
            return profile;
        }
        public static ProfileModel GetClientById(int UserId)
        {
            ProfileModel client = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string clientQuery = "select u.UserId , c.ClientID , u.FirstName , u.LastName , u.UserEmail , u.UserPhone , u.UserAddress , u.UserType , u.UserPhoto ," +
                    "c.CompanyName , c.ClientDescription , u.Country , u.DOB , c.Facebook , c.Linkedin , u.Languagee " +
                    "from Users u JOIN Client c ON u.UserId=c.UserId where u.UserId=@UserId";
                SqlCommand command = new SqlCommand(clientQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    client = new ProfileModel {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        UserEmail = reader["UserEmail"].ToString(),
                        UserPhone = reader["UserPhone"].ToString(),
                        UserAddress = reader["UserAddress"].ToString(),
                        UserPhoto = reader["UserPhoto"].ToString(),
                        CompanyName = reader["CompanyName"].ToString(),
                        ClientDescription = reader["ClientDescription"].ToString(),
                        Country = reader["Country"].ToString(),
                        DOB = reader["DOB"].ToString(),
                        Facebook = reader["Facebook"].ToString(),
                        LinkedIn = reader["Linkedin"].ToString(),
                        Languagee = reader["Languagee"].ToString()
                    };
                }
            }
            return client;
        }
        public static ProfileModel GetDeveloperById(int UserId)
        {
            ProfileModel client = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string clientQuery = "select u.UserId , c.DeveloperID , u.FirstName , u.LastName , u.UserEmail , u.UserPhone , u.UserAddress , u.UserType , u.UserPhoto ," +
                    " u.Country , u.DOB , c.Facebook , c.Linkedin , u.Languagee " +
                    "from Users u JOIN Developer c ON u.UserId=c.UserId where u.UserId=@UserId";
                SqlCommand command = new SqlCommand(clientQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    client = new ProfileModel
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        UserEmail = reader["UserEmail"].ToString(),
                        UserPhone = reader["UserPhone"].ToString(),
                        UserAddress = reader["UserAddress"].ToString(),
                        UserPhoto = reader["UserPhoto"].ToString(),
                        Country = reader["Country"].ToString(),
                        DOB = reader["DOB"].ToString(),
                        Facebook = reader["Facebook"].ToString(),
                        LinkedIn = reader["Linkedin"].ToString(),
                        Languagee = reader["Languagee"].ToString()
                    };
                }
            }
            return client;
        }

        public static int UpdateUser(string UserId, string FirstName, string LastName, string email, string address, string phone, string country, string lang, string dob, string Photo)
        {
            int rowsAffected = 0;
            // Connection string from web.config
            string connectionString = ConnectDB.connect;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, UserEmail = @Email, UserPhone = @Phone, UserAddress = @Address, Country = @Country, Languagee = @Language, DOB = @DateOfBirth, UserPhoto = @PhotoPath WHERE UserId = @Id";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Language", lang);
                    command.Parameters.AddWithValue("@DateOfBirth", dob);
                    command.Parameters.AddWithValue("@PhotoPath", Photo);
                    command.Parameters.AddWithValue("@Id", UserId);

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
        public static int UpdateDeveloper(string UserId, string DevDescription, string AreaofExpertise, string PortfolioLink, string LinkedIn, string Facebook)
        {
            int rowsAffected = 0;
            // Connection string from web.config
            string connectionString = ConnectDB.connect;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Developer SET DevDescription = @DevDescription, AreaofExpertise = @AreaofExpertise, PortfolioLink = @PortfolioLink, LinkedIn = @LinkedIn, Facebook = @Facebook WHERE UserId = @Id";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@DevDescription", DevDescription);
                    command.Parameters.AddWithValue("@AreaofExpertise", AreaofExpertise);
                    command.Parameters.AddWithValue("@PortfolioLink", PortfolioLink);
                    command.Parameters.AddWithValue("@LinkedIn", LinkedIn);
                    command.Parameters.AddWithValue("@Facebook", Facebook);
                    command.Parameters.AddWithValue("@Id", UserId);

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
        public static List<ProfileModel> GetProjectsByClientId(int clientId)
        {
            List<ProfileModel> projects = new List<ProfileModel>();
            string connectionString = ConnectDB.connect;

            string query = "SELECT * FROM Project WHERE ClientID = @ClientID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientID", clientId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    projects.Add(new ProfileModel
                    {
                        ProjectID = (int)reader["ProjectID"],
                        ProjectTitle = reader["ProjectTitle"].ToString(),
                        ShortDescription = reader["ShortDescription"].ToString(),
                        PostedOn = (DateTime)reader["PostedOn"],
                        // Add other properties as needed
                    });
                }
            }

            return projects;
        }
        public static List<ProfileModel> GetHandshakedProjectsByDeveloperId(int developerId)
        {
            List<ProfileModel> handshakedProjects = new List<ProfileModel>();

            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT p.ProjectID, p.ProjectTitle, h.HandshakeDate, h.Status, h.Duration
            FROM Handshake h
            INNER JOIN Project p ON h.ProjectID = p.ProjectID
            WHERE h.DeveloperID = @DeveloperID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DeveloperID", developerId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        handshakedProjects.Add(new ProfileModel
                        {
                            ProjectID = (int)reader["ProjectID"],
                            Duration = reader["Duration"].ToString(),
                            ProjectTitle = reader["ProjectTitle"].ToString(),
                            HandshakeDate = (DateTime)reader["HandshakeDate"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return handshakedProjects;
        }

        public static List<ProfileModel> GetHandshakedProjectsByClientId(int clientId)
        {
            List<ProfileModel> handshakedProjects = new List<ProfileModel>();

            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT p.ProjectID, p.ProjectTitle, h.HandshakeDate, h.Status, h.Duration
            FROM Handshake h
            INNER JOIN Project p ON h.ProjectID = p.ProjectID
            WHERE h.ClientID = @clientId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClientID", clientId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        handshakedProjects.Add(new ProfileModel
                        {
                            ProjectID = (int)reader["ProjectID"],
                            Duration = reader["Duration"].ToString(),
                            ProjectTitle = reader["ProjectTitle"].ToString(),
                            HandshakeDate = (DateTime)reader["HandshakeDate"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return handshakedProjects;
        }

        public static int AddWorkExperience(int UserID , string CompanyName , string Position , string startDate , string Enddate , string JobDescription)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Experience (UserId , CompanyName , Position , StartingYear , EndingYear , JobDescription) VALUES (@UserId,@CompanyName,@Position,@StartingYear,@EndingYear,@JobDescription)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@CompanyName", CompanyName);
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@StartingYear", startDate);
                    command.Parameters.AddWithValue("@EndingYear", Enddate);
                    command.Parameters.AddWithValue("@UserId", UserID);
                    command.Parameters.AddWithValue("@JobDescription", JobDescription);

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
            }

                return rowsAffected;
        }

        public static List<ProfileModel> FetchExperience(int userID)
        {
            List<ProfileModel> experiences = new List<ProfileModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName,Position,StartingYear,EndingYear,JobDescription FROM Experience where UserId=@userID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userID", userID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProfileModel experience = new ProfileModel
                    {
                        CompanyName = reader["CompanyName"].ToString(),
                        Position = reader["Position"].ToString(),
                        StartDate = reader["StartingYear"].ToString(),
                        EndDate = reader["EndingYear"].ToString(),
                        JobDescription = reader["JobDescription"].ToString()
                    };
                    experiences.Add(experience);
                }
            }

            return experiences;
        }
    }
}