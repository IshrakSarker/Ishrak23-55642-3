using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeDetails
{
    public class Employee
    {
        private static readonly string myConn =
            ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString;

        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string Age { get; set; }
        public string ContactNo { get; set; }
        public string Gender { get; set; }
        public int CreatedBy { get; set; }

        private const string SelectQuery = @"
            SELECT
                e.EmpId,
                e.EmpName,
                e.EmpAge,
                e.EmpContact,
                e.EmpGender,
                u.Username AS CreatedBy
            FROM dbo.Emp_details e
            LEFT JOIN dbo.Users u ON e.CreatedBy = u.UserID;";

        private const string InsertQuery = @"
            INSERT INTO dbo.Emp_details
                (EmpId, EmpName, EmpAge, EmpContact, EmpGender, CreatedBy)
            VALUES
                (@EmpId, @EmpName, @EmpAge, @EmpContact, @EmpGender, @CreatedBy);";

        private const string UpdateQuery = @"
            UPDATE dbo.Emp_details
            SET EmpName = @EmpName,
                EmpAge = @EmpAge,
                EmpContact = @EmpContact,
                EmpGender = @EmpGender
            WHERE EmpId = @EmpId;";

        private const string DeleteQuery =
            "DELETE FROM dbo.Emp_details WHERE EmpId = @EmpId;";

        public DataTable GetEmployees()
        {
            DataTable datatable = new DataTable();

            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(SelectQuery, con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                con.Open();
                adapter.Fill(datatable);
            }

            return datatable;
        }

        public bool InsertEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(InsertQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", employee.EmpId);
                cmd.Parameters.AddWithValue("@EmpName", employee.EmpName);
                cmd.Parameters.AddWithValue("@EmpAge", employee.Age);
                cmd.Parameters.AddWithValue("@EmpContact", employee.ContactNo);
                cmd.Parameters.AddWithValue("@EmpGender", employee.Gender);
                cmd.Parameters.AddWithValue("@CreatedBy", employee.CreatedBy);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(UpdateQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpName", employee.EmpName);
                cmd.Parameters.AddWithValue("@EmpAge", employee.Age);
                cmd.Parameters.AddWithValue("@EmpContact", employee.ContactNo);
                cmd.Parameters.AddWithValue("@EmpGender", employee.Gender);
                cmd.Parameters.AddWithValue("@EmpId", employee.EmpId);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            using (SqlCommand cmd = new SqlCommand(DeleteQuery, con))
            {
                cmd.Parameters.AddWithValue("@EmpId", employee.EmpId);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
