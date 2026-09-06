using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
            label1.Text = "Welcome, " + Session.Username;
        }

        private void visitWeb_Click(object sender, EventArgs e)
        {
            bmBrowser.Navigate("https://bloggingmetrics.com/");
        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            using (frmEmployee employeeForm = new frmEmployee())
            {
                employeeForm.ShowDialog();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            Session.Clear();

            frmLogin login = new frmLogin();
            login.Show();
            Close();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
        }
    }
}
