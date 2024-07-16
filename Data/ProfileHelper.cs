using ALlyHub.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALlyHub.Data
{
    public class ProfileHelper 
    {
        private static readonly string connectionString = "Data Source=USER\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";
        public static ProfileModel GetProfileById(int userId)
        {
            ProfileModel profile = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select u.FirstName , u.LastName, u.UserAddress , u.UserPhone , u.UserEmail , d.DeveloperID, d.UserId ,d.DevDescription , " +
                    "d.AreaofExpertise , d.PortfolioLink , d.LinkedIn ,d.Facebook, d.Country , d.Languagee , d.DOB from Developer d " +
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
                        //UserPhoto = reader["UserPhoto"].ToString()
                    };
                }

                connection.Close();
            }

            return profile;
        }

    }
}