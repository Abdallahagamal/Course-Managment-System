using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using CustomControls.RJControls;  // ✅ Add this to access RJTextBox
using System.Data.SqlClient;
namespace courseApp
{
    public partial class Form1 : Form
    {
        Boolean click_check = false;
        private int panel2OriginalY;
        private int panel3OriginalY;

        void handleicon(PictureBox icon)
        {
            List<PictureBox> iconList = new List<PictureBox> { homeicom2, examicon2, coursesicon2, classworkicon2, chaticon2, usericon2 };
            foreach (var icons in iconList)
            {
                icons.SendToBack();
            }

            icon.BringToFront();
            if (icon != usericon2)
            {
                click_check = false;
            }

        }

        private void UpdateScrollBar()
        {
            // 1. Find the total height of all the controls inside panel2
            int maxBottom = 0;
            foreach (Control ctrl in panel2.Controls)
            {
                if (ctrl.Bottom > maxBottom)
                    maxBottom = ctrl.Bottom;
            }

            // 2. Update scrollbar based on content height
            int visibleHeight = this.ClientSize.Height; // What user can see
            int contentHeight = maxBottom;

            vScrollBar1.Maximum = Math.Max(0, contentHeight - visibleHeight);
            vScrollBar1.LargeChange = visibleHeight;
            vScrollBar1.SmallChange = 20;

            // Only adjust Value if it's out of range
            if (vScrollBar1.Value > vScrollBar1.Maximum)
                vScrollBar1.Value = vScrollBar1.Maximum;
            // Do NOT set panel2.Location here!
        }
        private void AddCoursePanel(string courseName, Panel currentpanel)
        {
            // 1. Create the new panel
            Panel newCoursePanel = new Panel();
            newCoursePanel.Size = new Size(302, 286); // Adjust as needed
            newCoursePanel.BackColor = Color.White;

            // 2. Find position for the new panel
            int newY = 10; // Default top margin if no courses
            int newx = 0;
            if (currentpanel.Controls.Count > 0)
            {
                // Get the last panel's bottom position
                Control last = currentpanel.Controls[currentpanel.Controls.Count - 1];
                if (last.Location.X + 396 < currentpanel.Width)
                {
                    newx = last.Location.X + 379;
                    newY = last.Location.Y;
                }
                else
                {
                    newx = 17;
                    newY = last.Location.Y + 306;
                    panel2.Height += +406;
                }
            }

            newCoursePanel.Location = new Point(newx, newY);



            // 4. Add the panel to panel2
            newCoursePanel.Cursor = Cursors.Hand;
            panel2.Controls.Add(newCoursePanel);
            PictureBox newpic = new PictureBox();
            newpic.Size = new Size(251, 132);

            newpic.Location = new Point(25, 26);
            newpic.BackgroundImageLayout = ImageLayout.Stretch;
            newpic.BackgroundImage = Image.FromFile("C:\\Users\\Abdallah Gamal\\OneDrive\\Desktop\\123.png");

            newCoursePanel.Controls.Add(newpic);

            currentpanel.Controls.Add(newCoursePanel);
            RichTextBox newtitle = new RichTextBox();
            newtitle.Size = new Size(251, 94);

            newtitle.Location = new Point(25, 176);
            newtitle.Text = courseName;
            newtitle.ForeColor = Color.FromArgb(5, 12, 22);
            newtitle.Font = new Font("Montserrat Light", 18, FontStyle.Regular);
            newtitle.ReadOnly = true;
            newtitle.BackColor = Color.White;
            newtitle.BorderStyle = BorderStyle.None;
            newCoursePanel.Controls.Add(newtitle);
            // 5. Update scrollbar
            UpdateScrollBar();

        }



        public Form1()
        {


            List<string> myCourses = new List<string> { "Math", "Physics", "Programming" };

            InitializeComponent();
            string courseName = "Course " + (panel2.Controls.Count + 1);



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
            //string query = "SELECT Title FROM Course";
            panel2OriginalY = panel2.Location.Y; // Save original position
            panel3OriginalY = panel3.Location.Y;
            vScrollBar1.Minimum = 0;
            vScrollBar1.LargeChange = this.ClientSize.Height;
            vScrollBar1.SmallChange = 20;
            vScrollBar1.Maximum = Math.Max(0, panel2.Height - this.ClientSize.Height);
            vScrollBar2.Minimum = 0;
            vScrollBar2.LargeChange = this.ClientSize.Height;
            vScrollBar2.SmallChange = 20;
            vScrollBar2.Maximum = Math.Max(0, panel3.Height - this.ClientSize.Height);
            string targetWord = "learn";
            int startIndex = richTextBox1.Text.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                richTextBox1.Select(startIndex, targetWord.Length);
                richTextBox1.SelectionColor = Color.FromArgb(221, 168, 83); // Set to your desired color
                richTextBox1.Select(0, 0); // Deselect text
            }

            // Example: Fill a DataTable and bind to a DataGridView
            /*
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Title FROM Course", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    String title = row["Title"]?.ToString() ?? string.Empty;
                    // Use the title variable as needed
                    // Example: AddCoursePanel(title, panel2);
                }
            }
            */
            AddCoursePanel("title", panel2);




        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void homeicon_Click(object sender, EventArgs e)
        {
            handleicon(homeicom2);
            home.BringToFront();
            panel1.BringToFront();

        }

        private void classworkicon_Click(object sender, EventArgs e)
        {
            handleicon(classworkicon2);
            panel1.BringToFront();

        }

        private void coursesicon_Click(object sender, EventArgs e)
        {
            handleicon(coursesicon2);
            courses.BringToFront();
            panel1.BringToFront();

        }

        private void chaticon_Click(object sender, EventArgs e)
        {
            handleicon(chaticon2);
            panel1.BringToFront();


        }

        private void examicon_Click(object sender, EventArgs e)
        {
            handleicon(examicon2);
            panel1.BringToFront();


        }

        private void usericon_MouseEnter(object sender, EventArgs e)
        {
            usericon2.BringToFront();
        }

        private void usericon2_MouseHover(object sender, EventArgs e)
        {
            if (!click_check)
                usericon.BringToFront();
        }

        private void usericon_Click(object sender, EventArgs e)
        {
            usericon2.BringToFront();
        }

        private void usericon2_Click(object sender, EventArgs e)
        {
            handleicon(usericon2);
            click_check = true;
        }

        private void rjTextBox2__TextChanged(object sender, EventArgs e)
        {

        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        private void rjTextBox8__TextChanged(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel2.Location = new Point(panel2.Location.X, panel2OriginalY - vScrollBar1.Value);


        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            panel3.Location = new Point(panel3.Location.X, panel3OriginalY - vScrollBar2.Value);
        }

        private void searchbutton_Click(object sender, EventArgs e)
        {
            
         
        }
    }
}
