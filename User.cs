using System.Configuration;
using System.Data.SqlClient;

namespace EmployeeDetails
{
    public class User
    {
        private static readonly string myConn =
            ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString;

        public int ValidateLogin(string username, string password)
        {
            const string query = @"
                SELECT UserID
                FROM dbo.Users
                WHERE Username = @Username AND Password = @Password;";

            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                object result = cmd.ExecuteScalar();

                return result == null ? 0 : System.Convert.ToInt32(result);
            }
        }

        public bool UsernameExists(string username)
        {
            const string query = @"
                SELECT COUNT(1)
                FROM dbo.Users
                WHERE Username = @Username;";

            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();

                return System.Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool RegisterUser(string username, string password)
        {
            const string query = @"
                INSERT INTO dbo.Users (Username, Password)
                VALUES (@Username, @Password);";

            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
