using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmLogin : Form
    {
        private readonly User user = new User();

        public frmLogin()
        {
            InitializeComponent();
            this.FormClosed += frmLogin_FormClosed;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int userId = user.ValidateLogin(username, password);

                if (userId > 0)
                {
                    Session.UserID = userId;
                    Session.Username = username;

                    frmDashboard dashboard = new frmDashboard();
                    dashboard.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to log in.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = checkbxShowPas.Checked ? '\0' : '•';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void clickRegister_Click(object sender, EventArgs e)
        {
            frmRegister register = new frmRegister();
            register.Show();
            Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
