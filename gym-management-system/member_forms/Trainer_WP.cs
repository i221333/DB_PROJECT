using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Member_Forms.Trainer_Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Member_Forms
{
    public partial class Trainer_WP : Form
    {
        public Trainer_WP()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\trainer_workout.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Trainer_Layout form = new Trainer_Layout();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void LoadMemberIDs(int trainerid)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Member.ID,Member.FIRSTNAME FROM Member 
            JOIN MemberWorkout ON Member.ID = MemberWorkout.MemberID
            WHERE MemberWorkout.TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerid);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxMemberID.Items.Clear();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxMemberID.DataSource = new BindingSource(members, null);
                    comboBoxMemberID.DisplayMember = "Value";
                    comboBoxMemberID.ValueMember = "Key";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load member IDs: " + ex.Message);
            }
        }
        private void Trainer_WP_Load(object sender, EventArgs e)
        {
            int trainerid = SessionManager.TrainerId;
            LoadMemberIDs(trainerid);
        }

        private void comboBoxMemberID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMemberID.SelectedItem != null)
            {
                string selectedMember = comboBoxMemberID.SelectedItem.ToString();
                var match = Regex.Match(selectedMember, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedMemberId))
                {
                    CheckAndLoadWorkout(selectedMemberId);
                }
                else
                {
                    MessageBox.Show("Invalid member ID format.");
                }
            }
        }
        private void CheckAndLoadWorkout(int memberId)
        {
            int trainerid = SessionManager.TrainerId;
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT * FROM MemberWorkout WHERE MemberID = '" + memberId.ToString() + "'and TrainerID = '" + trainerid.ToString() + "'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox6.Text = reader["Title"].ToString();
                        textBox5.Text = reader["Goal"].ToString();
                        textBox4.Text = reader["Experience"].ToString();
                        textBox1.Text = reader["Schedule"].ToString();
                        MessageBox.Show("Work-out plan loaded for editing.");
                    }
                    else
                    {
                        MessageBox.Show("No work-out plan found for this member.");
                        ClearWorkoutFields();
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Diet Plan: " + ex.Message);
            }
        }

        private void ClearWorkoutFields()
        {
            textBox6.Clear();
            textBox5.Clear();
            textBox4.Clear();
            textBox1.Clear();
        }

        private void UpdateMemberWorkoutplan(int memberId, int trainerId, string title, string goal, string exp,string sch)
                               
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
            UPDATE MemberWorkout SET 
            Title = @Title,Goal =@Goal,Experience = @Experience,Schedule = @Schedule
            WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        cmd.Parameters.AddWithValue("@Title", title ?? (object)DBNull.Value);  // Handle nullable fields
                        cmd.Parameters.AddWithValue("@Goal", goal);
                        cmd.Parameters.AddWithValue("@Experience", exp);
                        cmd.Parameters.AddWithValue("@Schedule", sch);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Workout plan updated successfully.");
                        else
                            MessageBox.Show("No Workout plan was updated. Please check the member ID and trainer ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update diet plan: " + ex.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int trainerId = SessionManager.TrainerId;
            string selectedMember = comboBoxMemberID.SelectedItem.ToString();
            var match = Regex.Match(selectedMember, @"\d+");

            if (match.Success && int.TryParse(match.Value, out int selectedMemberId))
            {
             
                string title = textBox6.Text;
                string goal = textBox5.Text;
                string exp = textBox4.Text;
                string sch = textBox1.Text;

                UpdateMemberWorkoutplan(selectedMemberId, trainerId, title, goal, exp, sch);

            }
            else
            {
                MessageBox.Show("Invalid member ID format.");
            }
        }
    }
}

