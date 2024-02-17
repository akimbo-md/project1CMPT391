using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _391project1
{
    public partial class Form1 : Form
    {
        // Modify this depending on the database being used
        // For the remote database, the string is probably:
        // "Data Source = 206.75.31.209,11433 Initial Catalog = MacewanDatabase; Integrated Security = True; MultipleActiveResultSets = true;"
        private readonly string connectionString =
            "Data Source = localhost; Initial Catalog = MacewanDatabase; Integrated Security = True; MultipleActiveResultSets = true;";

        public Form1()
        {
            InitializeComponent();

            courseInfoListBox.Visible = true;

            fillSemBox();
            fillYearBox();

            listBox1.SelectedIndexChanged += ListBox_SelectionChanged;
            searchListBox.SelectedIndexChanged += ListBox_SelectionChanged;
            shoppingCartList.SelectedIndexChanged += ListBox_SelectionChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void myCoursesBtn_Click(object sender, EventArgs e)
        {
            // swaps view in tab page. functions the same as on click for tab control
        }

        private void changeBtn_Click(object sender, EventArgs e)
        {
            // swap between terms
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            // swaps view in tab page. functions the same as on click for tab control

        }

        private void cartBtn_Click(object sender, EventArgs e)
        {
            // swaps view in tab page. functions the same as on click for tab control
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //courseInfoLabel.Visible = false;
            showCourseInfo();
        }

        private int semIdx = -1;
        private int yrIdx = -1;

        private void fillSemBox()
        {
            SqlConnection con =
                new SqlConnection(connectionString);
            SqlCommand SelectCommand = new SqlCommand("SELECT distinct semester FROM section", con);
            SqlDataReader myreader;
            con.Open();

            myreader = SelectCommand.ExecuteReader();

            List<String> lstSemesters = new List<String>();
            while (myreader.Read())
            {
                lstSemesters.Add(myreader[0].ToString());

            }

            for (int i = 0; i < lstSemesters.Count; i++)
            {
                comboBoxSemester.Items.Add(lstSemesters[i]);
            }

            con.Close();
        }

        private string getTimeSlotID(string query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand SelectCommand = new SqlCommand(query, con);
                    using (SqlDataReader myreader = SelectCommand.ExecuteReader())
                    {
                        if (myreader.Read())
                        {
                            return myreader[0].ToString();
                        }
                        else
                        {
                            Debug.WriteLine("No results found for query: " + query);
                            return "-1";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Error: {ex.Message}");
                MessageBox.Show($"SQL Error: {ex.Message}");
                return "-1";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}");
                return "-1";
            }
        }


        private Boolean checkCart(string query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand SelectCommand = new SqlCommand(query, con);
                    using (SqlDataReader myreader = SelectCommand.ExecuteReader())
                    {
                        if (!myreader.Read())
                        {
                            return true; // no results found. can add class
                        }
                        else return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors appropriately
                MessageBox.Show("An 5 error occurred: " + ex.Message);
                return false;
            }

        }

        private string[] formatSearchClick(string[] raw)
        {
            string[] str = new string[11];


            int length = raw.Length;
            Debug.WriteLine("length " + length);

            str[0] = raw[0]; // courseID

            str[11] = raw[raw.Length - 1]; // year
            str[10] = raw[raw.Length - 2]; // sem
            str[9] = raw[raw.Length - 3]; //end
            str[8] = raw[raw.Length - 4]; //start
            str[7] = raw[raw.Length - 5]; //last name
            str[6] = raw[raw.Length - 6]; // first name
            str[5] = raw[raw.Length - 7]; //day
            str[4] = raw[raw.Length - 8]; //cap
            str[3] = raw[raw.Length - 9]; //enrolled
            str[2] = raw[raw.Length - 10]; //section
            str[1] = raw[
                raw.Length - 11]; // course desc. will only be containing last word, as it is uneeded for my use
            // change later if needed

            return str;
        }

        private void fillYearBox()
        {
            SqlConnection con =
                new SqlConnection(connectionString);
            SqlCommand SelectCommand = new SqlCommand("SELECT distinct year FROM section ORDER BY year DESC;", con);
            SqlDataReader myreader;
            con.Open();

            myreader = SelectCommand.ExecuteReader();

            List<String> lstYears = new List<String>();
            while (myreader.Read())
            {
                lstYears.Add(myreader[0].ToString());

            }

            for (int i = 0; i < lstYears.Count; i++)
            {
                comboBoxYear.Items.Add(lstYears[i]);
            }

            con.Close();
        }

        private void showCurrentCourses()
        {
            try
            {
                string act = "active";
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand SelectCommand = new SqlCommand("showMyCourses", con);
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.Parameters.AddWithValue("@studentID", UserLogin.GlobalVariables.userID.ToString());
                SqlDataReader myreader;
                con.Open();
                List<string> course = new List<string>();

                Debug.WriteLine(semIdx + "   " + yrIdx);

                if (semIdx != -1 & yrIdx != -1)
                {
                    listBox1.Items.Clear();
                    SelectCommand.Parameters.AddWithValue("@semester", comboBoxSemester.SelectedItem.ToString());
                    SelectCommand.Parameters.AddWithValue("@year", comboBoxYear.SelectedItem.ToString());

                    myreader = SelectCommand.ExecuteReader();
                    while (myreader.Read())
                    {
                        course.Add(myreader[0].ToString() + " | " + myreader[1].ToString() + " | " +
                                   myreader[2].ToString() + " | " + myreader[3].ToString());
                    }

                    foreach (var item in course)
                    {
                        listBox1.Items.Add(item);
                    }

                    con.Close();
                }
                else
                {
                    MessageBox.Show("select a semester and a year");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void showCurrentCart()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand SelectCommand = new SqlCommand("showMyCart", con))
                    {
                        SelectCommand.CommandType = CommandType.StoredProcedure;
                        SelectCommand.Parameters.AddWithValue("@studentID",
                            UserLogin.GlobalVariables.userID.ToString());
                        con.Open();

                        using (SqlDataReader myreader = SelectCommand.ExecuteReader())
                        {
                            // Clear the shopping cart list
                            shoppingCartList.Items.Clear();

                            // Read and add each course
                            while (myreader.Read())
                            {
                                string courseEntry = myreader[0].ToString() + " | " +
                                                     myreader[1].ToString() + " | " +
                                                     myreader[2].ToString() + " | " +
                                                     myreader[3].ToString();
                                shoppingCartList.Items.Add(courseEntry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void fillSearch(string semester, string year, string courseIdPattern)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("viewAvailableCourses", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@courseIDPattern",
                            string.IsNullOrEmpty(courseIdPattern) ? (object)DBNull.Value : courseIdPattern);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<string> courseDetails = new List<string>();
                            while (reader.Read())
                            {
                                string courseDetail = $"{reader["courseID"].ToString().Trim()} " +
                                                      $"| {reader["courseName"].ToString().Trim()} " +
                                                      $"| {reader["sectionID"].ToString().Trim()} " +
                                                      $"| {reader["semester"].ToString().Trim()} " +
                                                      $"| {reader["year"].ToString().Trim()} " +
                                                      $"| {reader["capacity"].ToString().Trim()} " +
                                                      $"| {reader["enrolledCount"].ToString().Trim()}";

                                courseDetails.Add(courseDetail);
                            }

                            searchListBox.Items.Clear();
                            foreach (var detail in courseDetails)
                            {
                                searchListBox.Items.Add(detail);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL error: {ex.Message}");
            }
        }

        private void addCourseToCart(string studentID, string courseID, string sectionID, string semester, string year)
        {
            try
            {
                // Check for time slot conflicts
                string timeSlotQuery =
                    $"SELECT timeSlotID FROM section WHERE courseID = '{courseID}' AND sectionID = '{sectionID}'";
                string newTimeSlotID = getTimeSlotID(timeSlotQuery);
                if (HasTimeSlotConflict(studentID, newTimeSlotID, semester, year))
                {
                    MessageBox.Show("Cannot add this course to cart due to a schedule conflict.");
                    return;
                }

                // Check for prerequisite completion
                List<string> prerequisiteCourseIDs = GetPrerequisiteCourseIDs(courseID);
                foreach (string prereqCourseID in prerequisiteCourseIDs)
                {
                    if (!HasStudentCompletedCourse(studentID, prereqCourseID))
                    {
                        MessageBox.Show($"You have not completed the prerequisite course: {prereqCourseID}");
                        return;
                    }
                }

                // Check if course already exists in cart
                string checkCartQuery = "SELECT COUNT(*) FROM cart WHERE studentID = @studentID AND courseID = @courseID AND sectionID = @sectionID AND semester = @semester AND year = @year";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(checkCartQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        cmd.Parameters.AddWithValue("@sectionID", sectionID);
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@year", year);

                        int existingCount = (int)cmd.ExecuteScalar();
                        if (existingCount > 0)
                        {
                            MessageBox.Show("Course already exists in your cart.");
                            return;
                        }
                    }
                }

                // Add to the cart
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("addToCart", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        cmd.Parameters.AddWithValue("@sectionID", sectionID);
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@year", year);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Course added to cart successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the course to cart: {ex.Message}");
            }
        }



        private void button1_Click(object sender, EventArgs e) // Add to cart button
        {
            Debug.WriteLine($"Selected Item: {searchListBox.SelectedItem}");

            if (searchListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a course to add.");
                return;
            }

            string selectedCourse = searchListBox.SelectedItem.ToString();
            string[] components = selectedCourse.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToArray();

            if (components.Length >= 6)
            {
                string courseID = components[0];
                string sectionID = components[2];
                string semester = components[3];
                string year = components[4];

                Debug.WriteLine("CourseID: " + courseID);
                Debug.WriteLine("SectionID: " + sectionID);
                Debug.WriteLine("Semester: " + semester);
                Debug.WriteLine("Year: " + year);

                string studentID = UserLogin.GlobalVariables.userID.ToString();
                addCourseToCart(studentID, courseID, sectionID, semester, year);
            }
            else
            {
                MessageBox.Show(
                    "Invalid course format selected. Please ensure the course information is in the expected format.");
            }
        }



        private void comboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedindex
            semIdx = comboBoxSemester.SelectedIndex;
            //Debug.WriteLine(semIdx.ToString());
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            yrIdx = comboBoxYear.SelectedIndex;
            //Debug.WriteLine(yrIdx.ToString());
        }

        private string fillListQuery =
            "SELECT courseID, sectionID, semester, year, capacity, instructorID, day from section";

        private void button1_Click_1(object sender, EventArgs e) //apply filter button
        {
            showCurrentCourses();
            showCurrentCart();
        }

        private void tabPageCart_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private string fillSearchQuery = "SELECT * FROM courses_schedule;";

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (comboBoxSemester.SelectedIndex != -1 && comboBoxYear.SelectedIndex != -1)
            {
                string semester = comboBoxSemester.SelectedItem.ToString().Trim();
                string year = comboBoxYear.SelectedItem.ToString().Trim();
                string courseIdPattern = searchTextBox.Text.Length > 0 ? searchTextBox.Text.Trim() : null;

                searchListBox.Items.Clear();
                fillSearch(semester, year, courseIdPattern);
            }
            else
            {
                MessageBox.Show("Please select both a semester and a year before searching.");
            }
        }

        private void viewCourseButton_Click(object sender, EventArgs e)
        {
            if (yrIdx != -1 & semIdx != -1)
            {
                courseInfoListBox.Visible = true;
                showCourseInfo();
            }

        }

        private void enrollStudentIntoCourse(string studentID, string courseID, string sectionID, string semester,
            string year)
        {
            try
            {
                Debug.WriteLine($"Student ID: {studentID}");
                Debug.WriteLine($"Course ID: {courseID}");
                Debug.WriteLine($"Section ID: {sectionID}");
                Debug.WriteLine($"Semester: {semester}");
                Debug.WriteLine($"Year: {year}");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO [dbo].[takes] ([studentID], [courseID], [sectionID], [semester], [year], [grade], [active]) " +
                        "VALUES (@studentID, @courseID, @sectionID, @semester, @year, NULL, 'ACTIVE')";
                    using (SqlCommand enrollCmd = new SqlCommand(query, con))
                    {
                        enrollCmd.Parameters.AddWithValue("@studentID", studentID);
                        enrollCmd.Parameters.AddWithValue("@courseID", courseID);
                        enrollCmd.Parameters.AddWithValue("@sectionID", sectionID);
                        enrollCmd.Parameters.AddWithValue("@semester", semester);
                        enrollCmd.Parameters.AddWithValue("@year", year);

                        con.Open();
                        enrollCmd.ExecuteNonQuery();
                    }
                }

                // Remove the course from the cart
                RemoveCourseFromCart(studentID, courseID, sectionID, semester, year);

                MessageBox.Show("Student enrolled successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while enrolling the student: {ex.Message}");
            }
        }

        private List<string> GetPrerequisiteCourseIDs(string courseID)
        {
            List<string> prerequisiteCourseIDs = new List<string>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT prereqID FROM prereq WHERE courseID = @courseID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@courseID", courseID.Trim().Replace("\u00A0", " ")); // get rid of bullshit

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string prereqID = reader["prereqID"].ToString().Trim();
                                prerequisiteCourseIDs.Add(prereqID);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Exception: {ex.Message}");
                MessageBox.Show($"An error occurred while retrieving prerequisite course IDs: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred while retrieving prerequisite course IDs: {ex.Message}");
            }

            return prerequisiteCourseIDs;
        }


        // Used for debugging, not needed
        private void ListCoursePrerequisites(string courseID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT prereqID FROM prereq WHERE courseID = @courseID",
                               con))
                    {
                        cmd.Parameters.AddWithValue("@courseID", courseID.Trim().Replace("\u00A0", " "));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string prerequisite = reader.GetString(0).Trim();
                                    Debug.WriteLine($"Prerequisite: {prerequisite}");
                                }
                            }
                            else
                            {
                                Debug.WriteLine("No prerequisites found.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Exception occurred while listing course prerequisites: {ex.Message}");
                MessageBox.Show($"An error occurred while listing course prerequisites: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while listing course prerequisites: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred while listing course prerequisites: {ex.Message}");
            }
        }

        private bool HasStudentCompletedCourse(string studentID, string courseID)
        {
            bool hasCompleted = false;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query =
                        "SELECT COUNT(1) FROM takes WHERE studentID = @studentID AND courseID = @courseID AND grade IS NOT NULL";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@courseID", courseID.Trim().Replace("\u00A0", " "));

                        int count = (int)cmd.ExecuteScalar();
                        hasCompleted = count > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Exception: {ex.Message}");
                MessageBox.Show($"An error occurred while checking course completion: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred while checking course completion: {ex.Message}");
            }

            return hasCompleted;
        }

        private bool HasTimeSlotConflict(string studentID, string newTimeSlotID, string semester, string year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check for conflict in currently enrolled courses
                    string queryEnrolled = @"
                        SELECT COUNT(1)
                        FROM takes
                        INNER JOIN section ON takes.courseID = section.courseID AND takes.sectionID = section.sectionID
                        WHERE takes.studentID = @studentID
                        AND section.timeSlotID = @timeSlotID
                        AND takes.semester = @semester
                        AND takes.year = @year";

                    using (SqlCommand cmdEnrolled = new SqlCommand(queryEnrolled, con))
                    {
                        cmdEnrolled.Parameters.AddWithValue("@studentID", studentID);
                        cmdEnrolled.Parameters.AddWithValue("@timeSlotID", newTimeSlotID);
                        cmdEnrolled.Parameters.AddWithValue("@semester", semester);
                        cmdEnrolled.Parameters.AddWithValue("@year", year);

                        int countEnrolled = (int)cmdEnrolled.ExecuteScalar();
                        if (countEnrolled > 0)
                        {
                            return true; // Conflict found in enrolled courses
                        }
                    }

                    // Check for conflict in the course cart
                    string queryCart = @"
                        SELECT COUNT(1)
                        FROM cart
                        INNER JOIN section ON cart.courseID = section.courseID AND cart.sectionID = section.sectionID
                        WHERE cart.studentID = @studentID
                        AND section.timeSlotID = @timeSlotID
                        AND cart.semester = @semester
                        AND cart.year = @year";

                    using (SqlCommand cmdCart = new SqlCommand(queryCart, con))
                    {
                        cmdCart.Parameters.AddWithValue("@studentID", studentID);
                        cmdCart.Parameters.AddWithValue("@timeSlotID", newTimeSlotID);
                        cmdCart.Parameters.AddWithValue("@semester", semester);
                        cmdCart.Parameters.AddWithValue("@year", year);

                        int countCart = (int)cmdCart.ExecuteScalar();
                        if (countCart > 0)
                        {
                            return true; // Conflict found in the course cart
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Exception: {ex.Message}");
                MessageBox.Show($"An error occurred while checking for time slot conflicts: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred while checking for time slot conflicts: {ex.Message}");
            }

            return false; // No conflict found
        }



        private void RemoveCourseFromCart(string studentID, string courseID, string sectionID, string semester,
            string year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("removeFromCart", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        cmd.Parameters.AddWithValue("@sectionID", sectionID);
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@year", year);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while removing the course from cart: {ex.Message}");
            }
        }


        private void showCourseInfo()
        {
            ListBox currentListBox = GetCurrentListBox();

            if (comboBoxYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a year.");
                return;
            }

            try
            {
                var selectedItem = currentListBox.SelectedItem.ToString(); // Get the current selection based on the current listbox
                var (courseID, sectionID, semester) = ParseSelectedItem(selectedItem);
                var year = comboBoxYear.SelectedItem.ToString().Trim(); // Using the selected year from comboBoxYear

                Debug.WriteLine($"Parsed Course ID: {courseID}");
                Debug.WriteLine($"Parsed Section ID: {sectionID}");
                Debug.WriteLine($"Parsed Semester: {semester}");
                Debug.WriteLine($"Selected Year: {year}");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand SelectCommand = new SqlCommand("getCourseInfo", con))
                    {
                        SelectCommand.CommandType = CommandType.StoredProcedure;
                        SelectCommand.Parameters.AddWithValue("@courseID", courseID);
                        SelectCommand.Parameters.AddWithValue("@sectionID", sectionID);
                        SelectCommand.Parameters.AddWithValue("@semester", semester);
                        SelectCommand.Parameters.AddWithValue("@year", year);

                        con.Open();
                        using (SqlDataReader myreader = SelectCommand.ExecuteReader())
                        {
                            if (myreader.Read())
                            {
                                courseInfoListBox.Items.Clear();

                                string status =
                                    Convert.ToInt32(myreader["enrolledCount"].ToString().Trim()) <
                                    Convert.ToInt32(myreader["capacity"].ToString().Trim())
                                        ? "Open"
                                        : "Closed";

                                string courseDetail =
                                    $"{myreader["courseID"].ToString().Trim()} - {myreader["courseName"].ToString().Trim()}";
                                string daysAndTimes =
                                    $"Days and Times: {myreader["day"].ToString().Trim()}, {myreader.GetTimeSpan(myreader.GetOrdinal("startTime")).ToString()} to {myreader.GetTimeSpan(myreader.GetOrdinal("endTime")).ToString()}";
                                string instructor = $"Instructor: {myreader["instructorName"].ToString().Trim()}";
                                string seats =
                                    $"Seats: {myreader["enrolledCount"].ToString().Trim()} of {myreader["capacity"].ToString().Trim()}";
                                string courseStatus = $"Status: {status}";

                                courseInfoListBox.Items.Add(courseDetail);
                                courseInfoListBox.Items.Add(daysAndTimes);
                                courseInfoListBox.Items.Add(instructor);
                                courseInfoListBox.Items.Add(seats);
                                courseInfoListBox.Items.Add(courseStatus);
                            }
                            else
                            {
                                MessageBox.Show("No course info found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private (string courseID, string sectionID, string semester) ParseSelectedItem(string selectedItem)
        {
            string[] parts = selectedItem.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);

            string courseID = parts.Length > 0 ? parts[0] : string.Empty;
            string sectionID = parts.Length > 2 ? parts[2] : string.Empty;
            string semester = parts.Length > 3 ? parts[3] : string.Empty;

            return (courseID, sectionID, semester);
        }




        private void courseInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // courseInfoListBox.Visible = false;
            courseInfoListBox.Items.Clear();
        }

        private void shoppingCartList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EnrollButton_Click_1(object sender, EventArgs e)
        {
            foreach (var item in shoppingCartList.Items)
            {
                string selectedCourse = item.ToString();
                Debug.WriteLine($"Selected Course: {selectedCourse}");

                string[] components = selectedCourse.Split(new[] { "   " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToArray();

                Debug.WriteLine($"Number of components: {components.Length}");

                if (components.Length >= 4)
                {
                    string courseID = components[0];
                    string sectionID = components[1];
                    string semester = components[2];
                    string year = components[3];

                    Debug.WriteLine($"Course ID: {courseID}");
                    Debug.WriteLine($"Section ID: {sectionID}");
                    Debug.WriteLine($"Semester: {semester}");
                    Debug.WriteLine($"Year: {year}");

                    string studentID = UserLogin.GlobalVariables.userID.ToString();
                    enrollStudentIntoCourse(studentID, courseID, sectionID, semester, year);
                }
                else
                {
                    MessageBox.Show(
                        "Invalid course format selected. Please ensure the course information is in the expected format.");
                }

                showCurrentCourses();
            }
        }

        private void RemoveFromCart_Click(object sender, EventArgs e)
        {
            if (shoppingCartList.SelectedItem != null)
            {
                try
                {
                    string selectedCourse = shoppingCartList.SelectedItem.ToString();
                    string[] components = selectedCourse.Split(new[] { "   " }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToArray();

                    if (components.Length >= 4)
                    {
                        string studentID = UserLogin.GlobalVariables.userID.ToString();
                        string courseID = components[0];
                        string sectionID = components[1];
                        string semester = components[2];
                        string year = components[3];

                        RemoveCourseFromCart(studentID, courseID, sectionID, semester, year);
                        shoppingCartList.Items.Remove(selectedCourse);

                        MessageBox.Show("Course removed from cart successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Invalid course format selected.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while removing the course from the cart: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a course to remove from the cart.");
            }
        }

        private ListBox GetCurrentListBox()
        {
            if (listBox1.Visible)
            {
                return listBox1;
            }
            else if (searchListBox.Visible)
            {
                return searchListBox;
            }
            else if (shoppingCartList.Visible)
            {
                return shoppingCartList;
            }
            else
            {
                return listBox1;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void MyCoursesHeading_Click(object sender, EventArgs e)
        {

        }

        private void tabPageCart_Click_1(object sender, EventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, EventArgs e)
        {
            showCourseInfo();
        }
    }
}
