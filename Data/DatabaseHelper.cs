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

        public static int GetDeveloperIdByUserId(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DeveloperID FROM Developer WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
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
