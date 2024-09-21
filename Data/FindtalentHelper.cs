using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ALlyHub.Models;
using System.Data;

namespace ALlyHub.Data
{
    public class FindtalentHelper
    {
        ConnectDB db= new ConnectDB();

        private static readonly string connectionString = ConnectDB.connect;

        public static List<FindTalentModel> FetchTalents()
        {
            List<FindTalentModel> findTalents = new List<FindTalentModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select d.DeveloperID , d.UserId , u.FirstName , u.UserPhoto , u.LastName, u.UserAddress , u.UserPhone , u.UserEmail , d.DevDescription , d.AreaofExpertise , d.PortfolioLink , d.LinkedIn " +
                    "from Developer d " +
                    "JOIN Users u ON d.UserId = u.UserId";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FindTalentModel findTalent = new FindTalentModel
                        {
                            DeveloperID = Convert.ToInt32(reader["DeveloperID"]),
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
                            UserPhoto = reader["UserPhoto"].ToString()

                        };
                        findTalents.Add(findTalent);
                    }
                }
                connection.Close();
            }

            return findTalents;
        }

        public static FindTalentModel FetchTalentByID(int DeveloperID)
        {
            FindTalentModel findTalentModel = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select u.FirstName , u.LastName, u.UserAddress, u.UserPhoto , u.UserPhone , u.UserEmail , d.DeveloperID, d.UserId ,d.DevDescription , d.AreaofExpertise , d.PortfolioLink , d.LinkedIn ,d.Facebook, u.Country , u.Languagee , u.DOB " +
                    "from Developer d " +
                    "JOIN Users u ON d.UserId = u.UserId where DeveloperID=@DeveloperID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DeveloperID", DeveloperID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    findTalentModel = new FindTalentModel
                    {
                        DeveloperID = Convert.ToInt32(reader["DeveloperID"]),
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
                        Facebook = reader["Facebook"].ToString(),
                        Country = reader["Country"].ToString(),
                        Languagee = reader["Languagee"].ToString(),
                        DOB = reader["DOB"].ToString(),
                        UserPhoto = reader["UserPhoto"].ToString()
                    };

                }
                connection.Close();
            }
            return findTalentModel;
        }
        public static List<FindTalentModel> GetHandshakedProjectsByDeveloperId(int developerId)
        {
            List<FindTalentModel> handshakedProjects = new List<FindTalentModel>();

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
                        handshakedProjects.Add(new FindTalentModel
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
        public static List<FindTalentModel> FetchExperience(int userID)
        {
            List<FindTalentModel> experiences = new List<FindTalentModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName,Position,StartingYear,EndingYear,JobDescription FROM Experience where UserId=@userID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userID", userID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FindTalentModel experience = new FindTalentModel
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
        public static List<FindTalentModel> FetchReviews(int userID)
        {
            List<FindTalentModel> reviews = new List<FindTalentModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(@"SELECT r.ReviewText, r.CreatedAt, 
                                                     u.FirstName, u.LastName
                                                     FROM Reviews r 
                                                     INNER JOIN Users u ON u.UserId = r.ReviewerId
                                                     WHERE r.UserID = @userID", connection))
                {
                    command.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FindTalentModel review = new FindTalentModel
                            {
                                ReviewText = reader["ReviewText"]?.ToString(),
                                ReviewDate = reader["CreatedAt"] is DateTime createdAt
                                    ? createdAt.ToString("yyyy-MM-dd")
                                    : null,
                                ReviewerFName = reader["FirstName"]?.ToString(),
                                ReviewerLName = reader["LastName"]?.ToString()
                            };

                            reviews.Add(review);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (use a logger or handle appropriately)
                throw new Exception("Error fetching reviews", ex);
            }

            return reviews;
        }
    }
}