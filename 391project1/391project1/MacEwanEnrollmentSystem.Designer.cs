namespace _391project1
{
    partial class MacEwanEnrollmentSystem
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPageMyCourses = new TabPage();
            dropClassButton = new Button();
            MyCoursesHeading = new Label();
            listBox1 = new ListBox();
            tabPageSearch = new TabPage();
            searchButton = new Button();
            addToCart = new Button();
            searchTextBox = new TextBox();
            searchListBox = new ListBox();
            tabPageCart = new TabPage();
            RemoveFromCart = new Button();
            EnrollButton = new Button();
            shoppingCartList = new ListBox();
            courseInfoListBox = new ListBox();
            comboBoxSemester = new ComboBox();
            comboBoxYear = new ComboBox();
            applyFilter = new Button();
            tabControl1.SuspendLayout();
            tabPageMyCourses.SuspendLayout();
            tabPageSearch.SuspendLayout();
            tabPageCart.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Alignment = TabAlignment.Left;
            tabControl1.Controls.Add(tabPageMyCourses);
            tabControl1.Controls.Add(tabPageSearch);
            tabControl1.Controls.Add(tabPageCart);
            tabControl1.Location = new Point(11, 139);
            tabControl1.Margin = new Padding(2);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(787, 410);
            tabControl1.TabIndex = 6;
            // 
            // tabPageMyCourses
            // 
            tabPageMyCourses.Controls.Add(dropClassButton);
            tabPageMyCourses.Controls.Add(MyCoursesHeading);
            tabPageMyCourses.Controls.Add(listBox1);
            tabPageMyCourses.Location = new Point(27, 4);
            tabPageMyCourses.Margin = new Padding(2);
            tabPageMyCourses.Name = "tabPageMyCourses";
            tabPageMyCourses.Size = new Size(756, 402);
            tabPageMyCourses.TabIndex = 0;
            tabPageMyCourses.Text = "My Courses";
            tabPageMyCourses.UseVisualStyleBackColor = true;
            // 
            // dropClassButton
            // 
            dropClassButton.Location = new Point(630, 3);
            dropClassButton.Name = "dropClassButton";
            dropClassButton.Size = new Size(123, 39);
            dropClassButton.TabIndex = 3;
            dropClassButton.Text = "Drop Class";
            dropClassButton.UseVisualStyleBackColor = true;
            dropClassButton.Click += button1_Click_2;
            // 
            // MyCoursesHeading
            // 
            MyCoursesHeading.AutoSize = true;
            MyCoursesHeading.Location = new Point(15, 15);
            MyCoursesHeading.Name = "MyCoursesHeading";
            MyCoursesHeading.Size = new Size(208, 15);
            MyCoursesHeading.TabIndex = 1;
            MyCoursesHeading.Text = "Course ID | Section ID | Semester | Year";
            MyCoursesHeading.Click += MyCoursesHeading_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 15;
            listBox1.Items.AddRange(new object[] { "" });
            listBox1.Location = new Point(3, 48);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(750, 349);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // tabPageSearch
            // 
            tabPageSearch.Controls.Add(searchButton);
            tabPageSearch.Controls.Add(addToCart);
            tabPageSearch.Controls.Add(searchTextBox);
            tabPageSearch.Controls.Add(searchListBox);
            tabPageSearch.Location = new Point(27, 4);
            tabPageSearch.Margin = new Padding(2);
            tabPageSearch.Name = "tabPageSearch";
            tabPageSearch.Size = new Size(756, 402);
            tabPageSearch.TabIndex = 1;
            tabPageSearch.Text = "Search Courses";
            tabPageSearch.UseVisualStyleBackColor = true;
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.Transparent;
            searchButton.FlatAppearance.BorderColor = Color.Silver;
            searchButton.Location = new Point(501, 3);
            searchButton.Margin = new Padding(2);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(123, 39);
            searchButton.TabIndex = 2;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += searchButton_Click;
            // 
            // addToCart
            // 
            addToCart.Location = new Point(630, 3);
            addToCart.Margin = new Padding(2);
            addToCart.Name = "addToCart";
            addToCart.Size = new Size(123, 39);
            addToCart.TabIndex = 9;
            addToCart.Text = "Add to Cart";
            addToCart.UseVisualStyleBackColor = true;
            addToCart.Click += button1_Click;
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(3, 12);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(492, 23);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += searchTextBox_TextChanged;
            // 
            // searchListBox
            // 
            searchListBox.FormattingEnabled = true;
            searchListBox.HorizontalScrollbar = true;
            searchListBox.ItemHeight = 15;
            searchListBox.Location = new Point(3, 48);
            searchListBox.Name = "searchListBox";
            searchListBox.Size = new Size(750, 349);
            searchListBox.TabIndex = 0;
            searchListBox.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // tabPageCart
            // 
            tabPageCart.Controls.Add(RemoveFromCart);
            tabPageCart.Controls.Add(EnrollButton);
            tabPageCart.Controls.Add(shoppingCartList);
            tabPageCart.Location = new Point(27, 4);
            tabPageCart.Margin = new Padding(2);
            tabPageCart.Name = "tabPageCart";
            tabPageCart.Size = new Size(756, 402);
            tabPageCart.TabIndex = 2;
            tabPageCart.Text = "Shopping Cart";
            tabPageCart.UseVisualStyleBackColor = true;
            // 
            // RemoveFromCart
            // 
            RemoveFromCart.Location = new Point(501, 3);
            RemoveFromCart.Name = "RemoveFromCart";
            RemoveFromCart.Size = new Size(123, 39);
            RemoveFromCart.TabIndex = 2;
            RemoveFromCart.Text = "Remove";
            RemoveFromCart.UseVisualStyleBackColor = true;
            RemoveFromCart.Click += RemoveFromCart_Click;
            // 
            // EnrollButton
            // 
            EnrollButton.Location = new Point(630, 3);
            EnrollButton.Name = "EnrollButton";
            EnrollButton.Size = new Size(123, 39);
            EnrollButton.TabIndex = 1;
            EnrollButton.Text = "Confirm Enroll";
            EnrollButton.UseVisualStyleBackColor = true;
            EnrollButton.Click += EnrollButton_Click_1;
            // 
            // shoppingCartList
            // 
            shoppingCartList.FormattingEnabled = true;
            shoppingCartList.ItemHeight = 15;
            shoppingCartList.Location = new Point(3, 48);
            shoppingCartList.Name = "shoppingCartList";
            shoppingCartList.Size = new Size(750, 349);
            shoppingCartList.TabIndex = 0;
            shoppingCartList.SelectedIndexChanged += shoppingCartList_SelectedIndexChanged;
            // 
            // courseInfoListBox
            // 
            courseInfoListBox.BackColor = SystemColors.Window;
            courseInfoListBox.ColumnWidth = 525;
            courseInfoListBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            courseInfoListBox.FormattingEnabled = true;
            courseInfoListBox.ItemHeight = 15;
            courseInfoListBox.Location = new Point(38, 42);
            courseInfoListBox.Margin = new Padding(2, 1, 2, 1);
            courseInfoListBox.Name = "courseInfoListBox";
            courseInfoListBox.Size = new Size(760, 94);
            courseInfoListBox.TabIndex = 4;
            courseInfoListBox.Visible = false;
            courseInfoListBox.SelectedIndexChanged += courseInfoListBox_SelectedIndexChanged;
            // 
            // comboBoxSemester
            // 
            comboBoxSemester.FormattingEnabled = true;
            comboBoxSemester.Location = new Point(38, 11);
            comboBoxSemester.Margin = new Padding(2);
            comboBoxSemester.Name = "comboBoxSemester";
            comboBoxSemester.Size = new Size(132, 23);
            comboBoxSemester.TabIndex = 7;
            comboBoxSemester.Text = "Semester";
            comboBoxSemester.SelectedIndexChanged += comboBoxSemester_SelectedIndexChanged;
            // 
            // comboBoxYear
            // 
            comboBoxYear.FormattingEnabled = true;
            comboBoxYear.Location = new Point(174, 11);
            comboBoxYear.Margin = new Padding(2);
            comboBoxYear.Name = "comboBoxYear";
            comboBoxYear.Size = new Size(132, 23);
            comboBoxYear.TabIndex = 8;
            comboBoxYear.Text = "Year";
            comboBoxYear.SelectedIndexChanged += comboBoxYear_SelectedIndexChanged;
            // 
            // applyFilter
            // 
            applyFilter.Location = new Point(668, 9);
            applyFilter.Margin = new Padding(2);
            applyFilter.Name = "applyFilter";
            applyFilter.Size = new Size(130, 24);
            applyFilter.TabIndex = 10;
            applyFilter.Text = "Apply";
            applyFilter.UseVisualStyleBackColor = true;
            applyFilter.Click += button1_Click_1;
            // 
            // MacEwanEnrollmentSystem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(816, 560);
            Controls.Add(applyFilter);
            Controls.Add(courseInfoListBox);
            Controls.Add(comboBoxYear);
            Controls.Add(comboBoxSemester);
            Controls.Add(tabControl1);
            Margin = new Padding(2);
            Name = "MacEwanEnrollmentSystem";
            Text = "MacEwanEnrollmentSystem";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPageMyCourses.ResumeLayout(false);
            tabPageMyCourses.PerformLayout();
            tabPageSearch.ResumeLayout(false);
            tabPageSearch.PerformLayout();
            tabPageCart.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button buttonMyCourses;
        private Button buttonSearchCourses;
        private Button buttonShoppingCart;
        private TabControl tabControl1;
        private TabPage tabPageMyCourses;
        private ListBox listBox1;
        private TabPage tabPageSearch;
        private TabPage tabPageCart;
        private ComboBox comboBoxSemester;
        private ComboBox comboBoxYear;
        private Button addToCart;
        private Button applyFilter;
        private TextBox searchTextBox;
        private Button searchButton;
        private ListBox shoppingCartList;
        private ListBox searchListBox;
        private Label MyCoursesHeading;
        private Button viewCourseButton;
        private ListBox courseInfoListBox;
        private Button EnrollButton;
        private Button Remove;
        private Button RemoveFromCart;
        private Button dropClassButton;
    }
}