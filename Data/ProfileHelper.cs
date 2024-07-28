using ALlyHub.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALlyHub.Data
{
    public class ProfileHelper 
    {
        private static readonly string connectionString = ConnectDB.connect;

        //private static readonly string connectionString = "Data Source=USER\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";
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

        public static int UpdateUser(int UserId , string FirstName, string LastName, string email, string address, string phone,string country,string lang,string dob, string Photo)
        {
            int rowsAffected = 0;
                // Connection string from web.config
                string connectionString = ConnectDB.connect;

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
                        command.Parameters.AddWithValue("@PhotoPath", Photo); // Assuming there's a PhotoPath property
                        command.Parameters.AddWithValue("@Id", UserId);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                        connection.Close();
                    
                }
                // Optionally, redirect to a different page after successful update
            return rowsAffected;

        }
    }
}