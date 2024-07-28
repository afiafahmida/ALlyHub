using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ALlyHub.Models;

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
                string query = "select u.FirstName , u.LastName, u.UserAddress, u.UserPhoto , u.UserPhone , u.UserEmail , d.DeveloperID, d.UserId ,d.DevDescription , d.AreaofExpertise , d.PortfolioLink , d.LinkedIn ,d.Facebook, d.Country , d.Languagee , d.DOB " +
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
    }
}