using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using BillingSystemDesign;
using BillingSystemDesgin;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace BillingSystemDesign
{
    public partial class LoginPage : Form
    {
        private string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";
        private const int AdminContactNumber = 011504;
        private const int AdminPassword = 1234;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(this);
            registerForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int enteredContactNumber;
            int enteredPassword;

            int.TryParse(textBox1.Text, out enteredContactNumber);
            int.TryParse(textBox2.Text, out enteredPassword);

            if (enteredContactNumber == AdminContactNumber && enteredPassword == AdminPassword)
            {
                MessageBox.Show("Admin login successful!");
                OpenBlankForm();
                this.Hide();
            }
            else
            {
                if (CheckRegisteredAccount(enteredContactNumber, enteredPassword))
                {
                    OpenPaymentPage(enteredContactNumber, enteredPassword);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                }
            }
        }

        private bool CheckRegisteredAccount(int enteredContactNumber, int enteredPassword)
        {
            string query = "SELECT COUNT(*) FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactNumber", enteredContactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", enteredPassword);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }
        private void OpenBlankForm()
        {
            Blank blankForm = new Blank(this); 
            blankForm.Show();
            this.Hide();
        }

        private void OpenPaymentPage(int contactNumber, int contactPassword)
        {
            PaymentPage paymentPage = new PaymentPage
            {
                ContactNumber = contactNumber,
                ContactPassword = contactPassword
            };
            paymentPage.SetContactName();
            paymentPage.Show();
            this.Hide();
        }
    }
}