using System;
using System.Data.SqlClient;

namespace Sesion
{
    public static class DatabaseManager
    {
        private const string connectionString = @"Data Source=DESKTOP-MS7L59C;Initial Catalog=Software;Integrated Security=True";


        public static void SaveUser(string username, string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users(Username, Email, Password, DateJoined, IsActive) VALUES (@Username, @Email, @Password, @DateJoined, @IsActive)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@DateJoined", DateTime.Now);
                    command.Parameters.AddWithValue("@IsActive", true); // Puedes cambiar esto según tus necesidades
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar usuario en la base de datos: " + ex.Message);
                // Aquí puedes manejar el error de acuerdo a tus necesidades
            }
        }
    }
}
