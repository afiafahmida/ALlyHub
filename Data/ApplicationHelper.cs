using ALlyHub.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ApplicationHelper
    {
        private static readonly string connectionString = ConnectDB.connect;

        public static void InsertApplicant(Project applicant)
        {
            string query = @"
                INSERT INTO Applicants 
                (ProjectID, ClientID, DeveloperID, ApplicantsName, ApplicantsEmail, ApplicantsWebsite, ApplicantsFile, ApplicantsCoverLetter) 
                VALUES 
                (@ProjectID, @ClientID, @DeveloperID, @ApplicantsName, @ApplicantsEmail, @ApplicantsWebsite, @ApplicantsFile, @ApplicantsCoverLetter)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProjectID", applicant.ProjectID);
                command.Parameters.AddWithValue("@ClientID", applicant.ClientID);
                command.Parameters.AddWithValue("@DeveloperID", applicant.DeveloperID);
                command.Parameters.AddWithValue("@ApplicantsName", applicant.ApplicantsName);
                command.Parameters.AddWithValue("@ApplicantsEmail", applicant.ApplicantsEmail);
                command.Parameters.AddWithValue("@ApplicantsWebsite", applicant.ApplicantsWebsite);
                command.Parameters.AddWithValue("@ApplicantsFile", applicant.ApplicantsFile);
                command.Parameters.AddWithValue("@ApplicantsCoverLetter", applicant.ApplicantsCoverLetter);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No rows were inserted.");
                }
            }
        }

        public static List<Applicant> GetApplicantsByProjectId(int projectId)
        {
            List<Applicant> applicants = new List<Applicant>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Applicants WHERE ProjectID = @ProjectID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProjectID", projectId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Applicant applicant = new Applicant
                    {
                        ApplicantID = Convert.ToInt32(reader["ApplicantID"]),
                        ProjectID = Convert.ToInt32(reader["ProjectID"]),
                        DeveloperID = Convert.ToInt32(reader["DeveloperID"]),
                        ApplicantsName = reader["ApplicantsName"].ToString(),
                        ApplicantsEmail = reader["ApplicantsEmail"].ToString(),
                        ApplicantsWebsite = reader["ApplicantsWebsite"].ToString(),
                        ApplicantsFile = reader["ApplicantsFile"].ToString(),
                        ApplicantsCoverLetter = reader["ApplicantsCoverLetter"].ToString(),
                    };

                    applicants.Add(applicant);
                }

                connection.Close();
            }

            return applicants;
        }

        public static bool InsertHandshake(Handshake handshake)
        {
            string query = "INSERT INTO Handshake (ProjectID,ClientID, DeveloperID, HandshakeDate, Status, Duration) " +
                           "VALUES (@ProjectID, @ClientID, @DeveloperID, @HandshakeDate, @Status, @Duration)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProjectID", handshake.ProjectID);
                cmd.Parameters.AddWithValue("@DeveloperID", handshake.DeveloperID);
                cmd.Parameters.AddWithValue("@ClientID", handshake.ClientID);
                cmd.Parameters.AddWithValue("@HandshakeDate", handshake.HandshakeDate);
                cmd.Parameters.AddWithValue("@Status", handshake.Status);
                cmd.Parameters.AddWithValue("@Duration", handshake.Duration);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;

                }
                catch (Exception ex)
                {
                    // Log or handle the error
                    return false;
                }
            }
        }


    }
}