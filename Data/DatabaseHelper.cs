using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ALlyHub.Data
{
    public class DatabaseHelper
    {
        //private static readonly string connectionString = "Data Source=USER\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";
        private static readonly string connectionString = "Data Source=ASHIK\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";
        
        public static int RegisterUser(string FirstName,string LastName, string email, string password, string address, string phone)
        {
            int newUserId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FirstName, LastName, UserEmail, UserPassword, UserPhone, UserAddress) OUTPUT INSERTED.UserId VALUES (@FirstName, @LastName, @Email, @Password, @Address, @Phone)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Phone", phone);
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
        public void RegisterClient(int userID)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Client (UserId) VALUES (@userID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userID);
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

        public static bool AuthenticateUser(string email, string password , out int userId)
        {
            
            userId= 0;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Query to Check Credentials
                string query = "SELECT UserID FROM Users WHERE UserEmail = @email AND UserPassword = @password";
                SqlCommand command= new SqlCommand(query, connection);

                //Add Parameters to Prevent SQL Injection
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = reader.GetInt32(0);
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
        }
    }
}