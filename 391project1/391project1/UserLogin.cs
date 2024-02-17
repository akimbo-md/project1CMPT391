using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _391project1
{
    public partial class UserLogin : Form
    {
        public UserLogin()
        {
            InitializeComponent();
        }

        void UserLogin_Load(object sender, EventArgs e)
        {

        }


        public static class GlobalVariables
        {
            public static string userID { get; set; }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            var userID = textBox1.Text;
            if (!string.IsNullOrEmpty(userID))
            {
                int check = checkUserId(userID);
                if (check == 1)
                {
                    GlobalVariables.userID = userID;
                    MacEwanEnrollmentSystem macEwanEnrollmentSystem = new MacEwanEnrollmentSystem();
                    macEwanEnrollmentSystem.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("User ID not recognized");
                }
            }
        }

        private int checkUserId(string userID)
        {
            int result = -1;
            string connectionString =
                "Data Source=localhost; Initial Catalog=MacewanDatabase; Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("checkStudentID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentID", userID);
                    con.Open();
                    using (SqlDataReader myreader = cmd.ExecuteReader())
                    {
                        if (myreader.Read())
                        {
                            result = int.Parse(myreader["Result"]
                                .ToString());
                        }
                    }
                }
            }
            return result;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
