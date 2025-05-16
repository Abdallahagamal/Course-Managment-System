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
        private int chatriginalY;
        ///////Global Varibales
        int RE_ID = 0;
        int SE_ID = 0;
        String Subject_temp;
        Boolean check_text=false;
        ///
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
        private PictureBox selectedChatPic = null;

        private void Chatpic_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic != null)
            {
                if (selectedChatPic != null)
                    selectedChatPic.Enabled = true;

                pic.Enabled = false;
                selectedChatPic = pic;
                RE_ID = (int)pic.Tag;
                loadMessages((int)pic.Tag, 123);

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
            int newY = 10;
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
            //newpic.BackgroundImage = Image.FromFile("C:\\Users\\Abdallah Gamal\\OneDrive\\Desktop\\123.png");

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
        private void loadChat(String Name, int ID)
        {
            Panel newchat = new Panel();
            newchat.Size = new Size(282, 67);
            newchat.BackColor = Color.White;
            int newY = 43;
            int newx = 12;
            if (ChatContainer.Controls.Count > 0)
            {
                // Get the last panel's bottom position
                Control last = ChatContainer.Controls[ChatContainer.Controls.Count - 1];
                newY = last.Location.Y + 86;
            }
            newchat.Location = new Point(newx, newY);
            newchat.Cursor = Cursors.Hand;
            ChatContainer.Controls.Add(newchat);

            PictureBox icon = new PictureBox();
            icon.Size = new Size(81, 61);
            icon.BackColor = Color.White;
            icon.Location = new Point(3, 3);
            icon.BackgroundImage = Image.FromStream(new MemoryStream(Properties.Resources.User_Circle));
            icon.BackgroundImageLayout = ImageLayout.Zoom;
            icon.Tag = ID;
            newchat.Controls.Add(icon);
            icon.Click += Chatpic_Click;

            Label name = new Label();
            name.Location = new Point(74, 22);
            name.Size = new Size(200, 26);
            name.ForeColor = Color.FromArgb(5, 12, 22);
            name.BackColor = Color.White;
            name.Font = new Font("Montserrat Light", 14, FontStyle.Regular);
            name.Text = Name;
            newchat.Controls.Add(name);
            name.BringToFront();
        }
        private void loadsender_msg(String content)
        {
            RichTextBox msg = new RichTextBox();
            msg.Location = new Point(25, 176);
            msg.Text = content;
            msg.ForeColor = Color.FromArgb(5, 12, 22);
            msg.Font = new Font("Montserrat Light", 16, FontStyle.Regular);
            msg.ReadOnly = true;
            msg.BackColor = SystemColors.ActiveCaption;
            msg.BorderStyle = BorderStyle.None;
            // Set a maximum width for the message box
            int maxWidth = 400;
            Size preferredSize = TextRenderer.MeasureText(msg.Text, msg.Font, new Size(maxWidth, int.MaxValue), TextFormatFlags.WordBreak);

            msg.Width = Math.Min(preferredSize.Width + 10, maxWidth);
            msg.Height = preferredSize.Height + 10;
            int newY = 43;
            int newx = 595;
            if (panel5.Controls.Count > 0)
            {
                Control last = panel5.Controls[panel5.Controls.Count - 1];
                newY = last.Bottom + 15;

            }
            if (newx + msg.Width + 20 > panel5.Width)
            {
                newx = panel5.Width - msg.Width - 37;
                if (newx < 0) newx = 0;
            }

            msg.Location = new Point(newx, newY);

            panel5.Controls.Add(msg);

        }
        private void loadreciver_msg(String content)
        {

            RichTextBox msg2 = new RichTextBox();
            msg2.Location = new Point(25, 176);
            msg2.Text = content;
            msg2.ForeColor = Color.FromArgb(5, 12, 22);
            msg2.Font = new Font("Montserrat Light", 16, FontStyle.Regular);
            msg2.ReadOnly = true;
            msg2.BackColor = Color.Gainsboro;
            msg2.BorderStyle = BorderStyle.None;
            // Set a maximum width for the message box
            int maxWidth = 400;
            Size preferredSize = TextRenderer.MeasureText(msg2.Text, msg2.Font, new Size(maxWidth, int.MaxValue), TextFormatFlags.WordBreak);

            msg2.Width = Math.Min(preferredSize.Width + 10, maxWidth);
            msg2.Height = preferredSize.Height + 10;
            int newY = 43;
            int newx = 32;
            if (panel5.Controls.Count > 0)
            {
                Control last = panel5.Controls[panel5.Controls.Count - 1];
                newY = last.Bottom + 15;
            }
            if (newx + msg2.Width + 20 > panel5.Width)
            {
                newx = panel5.Width - msg2.Width - 37;
                if (newx < 0) newx = 0;
            }

            msg2.Location = new Point(newx, newY);

            panel5.Controls.Add(msg2);
        }
<<<<<<< HEAD
        ////////////////////////////////////
        private void loadMessages(int receiver, int sender)
        {
            panel5.Controls.Clear();

            string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Use parameters to avoid SQL injection and use the method arguments
                SqlDataAdapter adapter = new SqlDataAdapter(
                    @"SELECT Sender_id, Reciever_id, Content,Subject, Datee, timee 
              FROM Message 
              WHERE (Sender_id = @SenderId AND Reciever_id = @ReceiverId) 
                 OR (Sender_id = @ReceiverId AND Reciever_id = @SenderId)
              ORDER BY Datee, timee", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@SenderId", sender);
                adapter.SelectCommand.Parameters.AddWithValue("@ReceiverId", receiver);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    string content = row["Content"]?.ToString() ?? string.Empty;
                    int senderId = Convert.ToInt32(row["Sender_id"]);
                    string Subject = row["Subject"]?.ToString() ?? string.Empty;
                    Subject_label.Text = Subject;
                    if (senderId == sender)
                        loadsender_msg(content);
                    else
                        loadreciver_msg(content);
                }
            }
        }
        private void loadChats_inpanel(int sender)
        {
            string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Use parameters to avoid SQL injection and use the method arguments
                SqlDataAdapter adapter = new SqlDataAdapter(
                    @"SELECT Userr.FName + ' ' + Userr.LName AS fullname , Chat.Reciever_id
                    FROM Userr
                    INNER JOIN Chat ON Chat.Reciever_id = Userr.UserId
                    WHERE Chat.Sender_id = @SenderId", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@SenderId", sender);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    string name = row["fullname"]?.ToString() ?? string.Empty;
                    int rID = Convert.ToInt32(row["Reciever_id"]);
                    loadChat(name, rID);
                }
            }

        }
=======

        
        private void LoadClassWork()
        {
            // مسح المحتوى القديم في panel2 (ClassWork)
            //classwork.Controls.Clear();

            // مسافة بداية الكروت
            int yOffset = 180;

            // الاتصال بقاعدة البيانات



            
            
           string connectionString = "Server=DESKTOP-KN5SVN0;Database=Course_system;Trusted_Connection=True;";

            // SQL Query لقراءة البيانات من جدول ClassWork
            string query = "SELECT  CW.Title, CW.Duration, CW.Date, CW.Description, C.Title AS CourseTitle " +
                           "FROM ClassWork CW " +
                           "INNER JOIN Course C ON CW.CourseId = C.CourseId";

            // الاتصال بقاعدة البيانات باستخدام SqlConnection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
               

                conn.Open(); // فتح الاتصال

                // تنفيذ الاستعلام باستخدام SqlCommand
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // قراءة البيانات باستخدام SqlDataReader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // إنشاء كرت جديد لكل ClassWork
                            Panel card = new Panel
                            {
                                Size = new Size(700, 120), // تحديد حجم الكارت
                                Location = new Point(250, yOffset),
                                BackColor = Color.FromArgb(200, 200, 200),
                                Font = new Font("Montserrat-ExtraBold", 10)
                            };

                            // عنوان الكورس
                            Label lblCourse = new Label
                            {
                                Text = $"Course: {reader["CourseTitle"]}",
                                Location = new Point(10, 10),
                                AutoSize = true,
                                Font = new Font("Montserrat", 12, FontStyle.Bold)
                            };

                            // عنوان التمرين
                            Label lblTitle = new Label
                            {
                                Text = $"Title: {reader["Title"]}",
                                Location = new Point(10, 40),
                                AutoSize = true,
                                Font = new Font("Montserrat", 12, FontStyle.Bold)
                            };

                            // الوصف
                            Label lblDesc = new Label
                            {
                                Text = $"Description: {reader["Description"]}",
                                Location = new Point(10, 70),
                                AutoSize = true,
                                Font = new Font("Montserrat", 12, FontStyle.Bold)
                            };

                            // التاريخ
                            Label lblDate = new Label
                            {
                                Text = $"Date: {Convert.ToDateTime(reader["Date"]).ToShortDateString()}",
                                Location = new Point(400, 10),
                                AutoSize = true,
                                Font = new Font("Montserrat", 12, FontStyle.Bold)
                            };

                            // المدة
                            Label lblDuration = new Label
                            {
                                Text = $"Duration: {reader["Duration"]}",
                                Location = new Point(400, 40),
                                AutoSize = true,
                                Font = new Font("Montserrat", 12, FontStyle.Bold)
                            };

                            // إضافة العناصر للكرت
                            card.Controls.Add(lblCourse);
                            card.Controls.Add(lblTitle);
                            card.Controls.Add(lblDesc);
                            card.Controls.Add(lblDate);
                            card.Controls.Add(lblDuration);

                            // إضافة الكارت للـ panel2
                            classwork.Controls.Add(card);

                            // تحديث المسافة بين الكروت
                            yOffset += 140;
                        }
                    }
               
               
                }
            }

            // تحديث قيمة الـ ScrollBar بناءً على المحتوى
            vScrollBar1.Maximum = Math.Max(0, classwork.Height - this.ClientSize.Height);
        }

>>>>>>> 03f2efb36312b33e0497827112144abd1924bf3f
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
            string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
            panel2OriginalY = panel2.Location.Y; // Save original position
            panel3OriginalY = panel3.Location.Y;
            chatriginalY = ChatContainer.Location.Y;
            vScrollBar1.Minimum = 0;
            vScrollBar1.LargeChange = this.ClientSize.Height;
            vScrollBar1.SmallChange = 20;
            vScrollBar1.Maximum = Math.Max(0, panel2.Height - this.ClientSize.Height);
            vScrollBar2.Minimum = 0;
            vScrollBar2.LargeChange = this.ClientSize.Height;
            vScrollBar2.SmallChange = 20;
            vScrollBar2.Maximum = Math.Max(0, panel3.Height - this.ClientSize.Height);
            /////////////////////////////////////

            //////////////////////////////////
            string targetWord = "learn";
            int startIndex = richTextBox1.Text.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                richTextBox1.Select(startIndex, targetWord.Length);
                richTextBox1.SelectionColor = Color.FromArgb(221, 168, 83); // Set to your desired color
                richTextBox1.Select(0, 0); // Deselect text
            }

            // Example: Fill a DataTable and bind to a DataGridView



            AddCoursePanel("title", panel2);
<<<<<<< HEAD

=======
            loadChat("Eyad Nader");
            loadChat("AHMED Nader");
            loadChat("Wael Mohamed");
            loadChat("Wael Mohamed");
            loadChat("Wael Mohamed");
>>>>>>> 03f2efb36312b33e0497827112144abd1924bf3f


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
            classwork.BringToFront();
            LoadClassWork();
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
<<<<<<< HEAD
            chat.BringToFront();
            loadChats_inpanel(123);
=======

>>>>>>> 03f2efb36312b33e0497827112144abd1924bf3f
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

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sendbtn_Click(object sender, EventArgs e)
        {
            if (messagebar.Texts != "")
            {
                string msg = messagebar.Texts;
                loadsender_msg(msg);
                messagebar.Texts = "";

                // Insert message into database
                string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO Message (MessageId,Sender_id, Reciever_id,timee,Datee,Content,Subject ) 
                                           VALUES (@MessageId,@SenderId, @RecieverId, @Timee, @Datee,@Content,@Subject)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MessageId", 10);
                        cmd.Parameters.AddWithValue("@Subject", 10);
                        cmd.Parameters.AddWithValue("@SenderId", SE_ID); // Replace with actual sender id
                        cmd.Parameters.AddWithValue("@RecieverId", RE_ID); // Replace with actual receiver id
                        cmd.Parameters.AddWithValue("@Content", msg);
                        cmd.Parameters.AddWithValue("@Datee", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@Timee", DateTime.Now.TimeOfDay);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
            }
        }

        private void messagebar_Click(object sender, EventArgs e)
        {
            messagebar.Texts = "";
        }

<<<<<<< HEAD
        private void ChatContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            //if(addbar.Textchanged)
            if (addbar.Texts != "" && subjectbar.Texts != "" && int.TryParse(addbar.Texts, out int number))
            {
                string ID = addbar.Texts;

                addbar.Texts = "";

                // Insert message into database
                string connectionString = "Server=LAPTOP-I23IVTH3;Database=course_system;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO Chat (Sender_id, Reciever_id) 
                                           VALUES (@SenderId, @RecieverId)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {

                        cmd.Parameters.AddWithValue("@SenderId", SE_ID); // Replace with actual sender id
                        cmd.Parameters.AddWithValue("@RecieverId", Convert.ToInt32(ID)); // Replace with actual receiver id

                        cmd.ExecuteNonQuery();
                    }
                    string selectQuery = "SELECT Userr.FName + ' ' + Userr.LName AS fullname FROM Userr WHERE UserId = @RecieverId";
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@RecieverId", Convert.ToInt32(ID));

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["fullname"]?.ToString() ?? string.Empty;
                                loadChat(name, Convert.ToInt32(ID));
                            }
                        }
                    }

                }



            }
            else
            {
            }
        }

        private void addbar_Click(object sender, EventArgs e)
        {
            addbar.Texts = "";
        }

        private void subjectbar_Click(object sender, EventArgs e)
        {
            subjectbar.Texts = "";
        }

        private void addbar__TextChanged(object sender, EventArgs e)
=======
        private void label15_Click(object sender, EventArgs e)
>>>>>>> 03f2efb36312b33e0497827112144abd1924bf3f
        {

        }
    }
}
