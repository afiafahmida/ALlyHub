using ALlyHub.Data;
using ALlyHub.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

public class SearchHelper
{
    private static readonly string connectionString = ConnectDB.connect;

    // Method to search users in the database by keyword
    public List<Search> SearchUsers(string query)
    {
        List<Search> users = new List<Search>();

        // Create SQL query with LIKE operator for keyword search
        string sqlQuery = @"SELECT UserId, FirstName, LastName, UserEmail, UserPhone, ISNULL(UserPhoto, '') AS UserPhoto, UserType
                             FROM Users 
                             WHERE (FirstName LIKE @query OR LastName LIKE @query 
                             OR UserEmail LIKE @query OR UserPhone LIKE @query) AND UserType = 'Developer'";

        // Use SqlConnection and SqlCommand to query the database
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@query", "%" + query + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Search user = new Search
                    {
                        UserId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        UserEmail = reader.GetString(3),
                        UserPhone = reader.GetString(4),
                        UserPhoto = reader.IsDBNull(5) ? string.Empty : reader.GetString(5), // Handle null photo
                        UserType = reader.GetString(6) // Read the UserType value
                    };
                    users.Add(user);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            throw new Exception("Error fetching users from the database", ex);
        }

        return users;
    }


    public static Search FetchClientsByID(int ClientId)
    {
        Search findclient = null;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "select u.FirstName , u.LastName, u.UserAddress, u.UserPhoto , u.UserPhone , u.UserEmail , d.ClientID, d.UserId ,d.ClientDescription, d.LinkedIn ,d.Facebook, u.Country , u.Languagee , u.DOB " +
                "from Client d " +
                "JOIN Users u ON d.UserId = u.UserId where ClientID=@ClientID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClientID", ClientId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                findclient = new Search
                {
                    ClientID = Convert.ToInt32(reader["ClientID"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    UserAddress = reader["UserAddress"].ToString(),
                    UserPhone = reader["UserPhone"].ToString(),
                    UserEmail = reader["UserEmail"].ToString(),
                    ClientDescription = reader["ClientDescription"].ToString(),
                 
                    
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
        return findclient;
    }
    public List<Search> SearchProjects(string query)
    {
        List<Search> projects = new List<Search>();
        string sqlQuery = @"SELECT ProjectID, ProjectTitle, ClientID, ShortDescription
                        FROM Project
                        WHERE ProjectTitle LIKE @query OR ShortDescription LIKE @query";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@query", "%" + query + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Search project = new Search
                    {
                        ProjectID = reader.GetInt32(0),
                        ProjectTitle = reader.GetString(1),
                        ClientID = reader.GetInt32(2),
                        ShortDescription = reader.GetString(3) 
                    };
                    projects.Add(project);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching projects from the database", ex);
        }

        return projects;
    }


}

