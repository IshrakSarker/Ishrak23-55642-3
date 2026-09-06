using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmRegister : Form
    {
        private readonly User user = new User();

        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Register Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter them.",
                    "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtConPassword.Clear();
                txtPassword.Focus();
                return;
            }

            try
            {
                if (user.UsernameExists(username))
                {
                    MessageBox.Show("That username already exists. Please choose another.",
                        "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (user.RegisterUser(username, password))
                {
                    MessageBox.Show("Your account has been successfully created.",
                        "Registration Successful", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtConPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to register the account.\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            char value = checkbxShowPas.Checked ? '\0' : '•';
            txtPassword.PasswordChar = value;
            txtConPassword.PasswordChar = value;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConPassword.Clear();
            txtUsername.Focus();
        }

        private void clickLogin_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
        }
    }
}
