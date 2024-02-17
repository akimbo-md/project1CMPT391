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
    public partial class MacEwanEnrollmentSystem : Form
    {
        // Modify this depending on the database being used
        // For the remote database, the string is:
        private readonly string connectionString = "Data Source = 206.75.31.209,11433; " +
                    "Initial Catalog = MacewanDatabase; " +
                    "User ID = mckenzy; " +
                    "Password = 123456; " +
                    "MultipleActiveResultSets = true;";
        // Otherwise, use this for a local database:
        //private readonly string connectionString = "Data Source = localhost; Initial Catalog = MacewanDatabase; Integrated Security = True; MultipleActiveResultSets = true;";

        public MacEwanEnrollmentSystem()
        {
            InitializeComponent();

            courseInfoListBox.Visible = true;

            fillSemBox();
            fillYearBox();

            listBox1.SelectedIndexChanged += ListBox_SelectionChanged;
            searchListBox.SelectedIndexChanged += ListBox_SelectionChanged;
            shoppingCartList.SelectedIndexChanged += ListBox_SelectionChanged;

            tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabControl1.SelectedTab;

            // Refresh list based on the selected tab
            if (selectedTab.Name == "tabPageMyCourses")
            {
                showCurrentCourses();
            }
            else if (selectedTab.Name == "tabPageCart")
            {
                showCurrentCart();
            }
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
            // Call showCourseInfo with the sender ListBox
            showCourseInfo(sender as ListBox, e);
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
                    
                    // Get semester and year from combo box
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
            Debug.WriteLine("Preparing to show the current cart contents.");
            shoppingCartList.Items.Clear();

            string selectedSemester = comboBoxSemester.SelectedItem?.ToString().Trim();
            string selectedYear = comboBoxYear.SelectedItem?.ToString().Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand SelectCommand = new SqlCommand("showMyCart", con))
                    {
                        SelectCommand.CommandType = CommandType.StoredProcedure;
                        SelectCommand.Parameters.AddWithValue("@studentID", UserLogin.GlobalVariables.userID.ToString());
                        SelectCommand.Parameters.AddWithValue("@selectedSemester", selectedSemester ?? (object)DBNull.Value);
                        SelectCommand.Parameters.AddWithValue("@selectedYear", selectedYear ?? (object)DBNull.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(SelectCommand);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            string courseID = row["courseID"].ToString().Trim();
                            string semester = row["semester"].ToString().Trim();
                            string year = row["year"].ToString().Trim();

                            string courseEntry = $"{courseID} | {row["courseName"].ToString().Trim()} | {row["sectionID"].ToString().Trim()} | {semester} | {year}";
                            shoppingCartList.Items.Add(courseEntry);

                            Debug.WriteLine($"Adding to cart display: CourseID: {courseID}, Semester: {semester}, Year: {year}");
                        }
                    }
                }

                Debug.WriteLine($"Total {shoppingCartList.Items.Count} items added to the cart.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying cart contents: {ex.Message}");
                Debug.WriteLine($"Error in showCurrentCart: {ex.Message}");
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
                Debug.WriteLine($"Attempting to add course {courseID} to cart for student {studentID}");

                // Check for time slot conflicts
                string timeSlotQuery =
                    $"SELECT timeSlotID FROM section WHERE courseID = '{courseID}' AND sectionID = '{sectionID}'";
                string newTimeSlotID = getTimeSlotID(timeSlotQuery);

                Debug.WriteLine($"Time Slot ID for course {courseID}, section {sectionID}: {newTimeSlotID}");

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
                
                //Add to the cart if there is available space
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("checkCourseAvailability", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        cmd.Parameters.AddWithValue("@sectionID", sectionID);
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@year", year);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int capacity = int.Parse(reader["capacity"].ToString().Trim());
                                int enrolledCount = int.Parse(reader["enrolledCount"].ToString().Trim());

                                if (enrolledCount < capacity)
                                {
                                    // If there is space, add the course to the cart
                                    using (SqlCommand addCmd = new SqlCommand("addToCart", con))
                                    {
                                        addCmd.CommandType = CommandType.StoredProcedure;
                                        addCmd.Parameters.AddWithValue("@studentID", studentID);
                                        addCmd.Parameters.AddWithValue("@courseID", courseID);
                                        addCmd.Parameters.AddWithValue("@sectionID", sectionID);
                                        addCmd.Parameters.AddWithValue("@semester", semester);
                                        addCmd.Parameters.AddWithValue("@year", year);

                                        addCmd.ExecuteNonQuery();
                                        MessageBox.Show("Course added to cart successfully.");
                                        Debug.WriteLine($"Course {courseID} added to cart for student {studentID}");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("This course is full and cannot be added to your cart.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unable to check course availability. Please try again later.");
                                return;
                            }
                        }
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

            if (components.Length >= 4)
            {
                string courseID = components[0];
                string courseTitle = components[1];
                string sectionID = components[2];
                string semester = components[3];
                string year = components[4];

                Debug.WriteLine($"Adding to Cart: CourseID: {courseID}, SectionID: {sectionID}, Semester: {semester}, Year: {year}");

                string studentID = UserLogin.GlobalVariables.userID.ToString();
                addCourseToCart(studentID, courseID, sectionID, semester, year);
            }
            else
            {
                MessageBox.Show("Invalid course format selected. Please ensure the course information is in the expected format.");
            }
        }

        private void comboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            semIdx = comboBoxSemester.SelectedIndex;
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            yrIdx = comboBoxYear.SelectedIndex;
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
                showCourseInfo(sender, e);
            }

        }

        private void enrollStudentIntoCourse(string studentID, string courseID, string courseTitle, string sectionID, string semester, string year)
        {
            try
            {
                Debug.WriteLine($"Enrolling Student...\nStudent ID: {studentID}");
                Debug.WriteLine($"Course ID: {courseID}");
                Debug.WriteLine($"Course Title: {courseTitle}");
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
                shoppingCartList.Items.Remove($"{courseID} | {courseTitle} | {sectionID} | {semester} | {year}");

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

        private void showCourseInfo(object sender, EventArgs e)
        {
            ListBox currentListBox = sender as ListBox;

            if (comboBoxYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a year.");
                return;
            }

            try
            {
                if (currentListBox != null && currentListBox.SelectedItem != null)
                {
                    string selectedItem = currentListBox.SelectedItem.ToString();
                    var (courseID, sectionID, semester) = ParseSelectedItem(selectedItem);
                    string year = comboBoxYear.SelectedItem.ToString().Trim();

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

                                    string status = Convert.ToInt32(myreader["enrolledCount"].ToString().Trim()) < Convert.ToInt32(myreader["capacity"].ToString().Trim()) ? "Open" : "Closed";
                                    string courseDetail = $"{myreader["courseID"].ToString().Trim()} - {myreader["courseName"].ToString().Trim()}";
                                    string daysAndTimes = $"Days and Times: {myreader["day"].ToString().Trim()}, {myreader.GetTimeSpan(myreader.GetOrdinal("startTime")).ToString()} to {myreader.GetTimeSpan(myreader.GetOrdinal("endTime")).ToString()}";
                                    string instructor = $"Instructor: {myreader["instructorName"].ToString().Trim()}";
                                    string seats = $"Seats: {myreader["enrolledCount"].ToString().Trim()} of {myreader["capacity"].ToString().Trim()}";
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
            for (int i = shoppingCartList.Items.Count - 1; i >= 0; i--)
            {
                string selectedCourse = shoppingCartList.Items[i].ToString();
                Debug.WriteLine($"Selected Course for Enrollment: {selectedCourse}");

                string[] components = selectedCourse.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToArray();

                if (components.Length >= 4)
                {
                    string courseID = components[0];
                    string courseTitle = components[1];
                    string sectionID = components[2];
                    string semester = components[3];
                    string year = comboBoxYear.SelectedItem.ToString().Trim();

                    Debug.WriteLine($"Attempting to enrol... Course ID: {courseID}, Section ID: {sectionID}, Semester: {semester}, Year: {year}");

                    string studentID = UserLogin.GlobalVariables.userID.ToString();
                    enrollStudentIntoCourse(studentID, courseID, courseTitle, sectionID, semester, year);
                }
                else
                {
                    MessageBox.Show("Invalid course format selected. Please ensure the course information is in the expected format.");
                }
            }
        }

        private void RemoveFromCart_Click(object sender, EventArgs e)
        {
            if (shoppingCartList.SelectedItem != null)
            {
                try
                {
                    string selectedCourse = shoppingCartList.SelectedItem.ToString();
                    string[] components = selectedCourse.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToArray();

                    if (components.Length >= 4)
                    {
                        string studentID = UserLogin.GlobalVariables.userID.ToString();
                        string courseID = components[0];
                        string sectionID = components[2];
                        string semester = components[3];
                        string year = comboBoxYear.SelectedItem.ToString().Trim();

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
            showCourseInfo(sender, e);
        }
    }
}