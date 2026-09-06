using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmEmployee : Form
    {
        private readonly Employee employee = new Employee();

        public frmEmployee()
        {
            InitializeComponent();
            dgvEmployeeDetails.DataSource = employee.GetEmployees();
        }

        private bool ReadEmployeeFields()
        {
            if (string.IsNullOrWhiteSpace(txtEmpId.Text) ||
                string.IsNullOrWhiteSpace(txtEmpName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                cboGender.SelectedItem == null)
            {
                MessageBox.Show("Please fill in Employee ID, Name, Age and Gender.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtAge.Text, out _))
            {
                MessageBox.Show("Age must be a valid number.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            employee.EmpId = txtEmpId.Text.Trim();
            employee.EmpName = txtEmpName.Text.Trim();
            employee.Age = txtAge.Text.Trim();
            employee.ContactNo = txtContactNo.Text.Trim();
            employee.Gender = cboGender.SelectedItem.ToString();
            return true;
        }

        private void RefreshGrid()
        {
            dgvEmployeeDetails.DataSource = employee.GetEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ReadEmployeeFields())
                return;

            employee.CreatedBy = Session.UserID;

            try
            {
                bool success = employee.InsertEmployee(employee);
                RefreshGrid();

                if (success)
                {
                    MessageBox.Show("Employee has been added successfully.");
                    ClearControls();
                }
                else
                {
                    MessageBox.Show("Error occurred. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add employee.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ReadEmployeeFields())
                return;

            try
            {
                bool success = employee.UpdateEmployee(employee);
                RefreshGrid();

                if (success)
                {
                    MessageBox.Show("Employee has been updated successfully.");
                    ClearControls();
                }
                else
                {
                    MessageBox.Show("Error occurred. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update employee.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpId.Text))
            {
                MessageBox.Show("Select an employee first.",
                    "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this employee?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            employee.EmpId = txtEmpId.Text.Trim();

            try
            {
                bool success = employee.DeleteEmployee(employee);
                RefreshGrid();

                if (success)
                {
                    MessageBox.Show("Employee has been deleted successfully.");
                    ClearControls();
                }
                else
                {
                    MessageBox.Show("Error occurred. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete employee.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtEmpId.Clear();
            txtEmpName.Clear();
            txtAge.Clear();
            txtContactNo.Clear();
            cboGender.SelectedIndex = -1;
        }

        private void dgvEmployeeDetails_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvEmployeeDetails.Rows[e.RowIndex];

            txtEmpId.Text = Convert.ToString(row.Cells["EmpId"].Value);
            txtEmpName.Text = Convert.ToString(row.Cells["EmpName"].Value);
            txtAge.Text = Convert.ToString(row.Cells["EmpAge"].Value);
            txtContactNo.Text = Convert.ToString(row.Cells["EmpContact"].Value);
            cboGender.Text = Convert.ToString(row.Cells["EmpGender"].Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void cboGender_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dgvEmployeeDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtEmpId_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
