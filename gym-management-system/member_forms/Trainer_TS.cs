using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static Member_Forms.Trainer_Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Member_Forms
{
    public partial class Trainer_TS : Form
    {
        public Trainer_TS()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\appointment.jpg");
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

        private void Trainer_TS_Load(object sender, EventArgs e)
        {
            int trainerId = SessionManager.TrainerId;
            LoadGetTrainedMembers(trainerId);
        }

        private void LoadGetTrainedMembers(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT Member.ID,Member.FIRSTNAME FROM Member
            JOIN TrainerMembers ON Member.ID = TrainerMembers.MemberID
            WHERE TrainerMembers.TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["ID"].ToString());
                        string name = reader["FIRSTNAME"].ToString();
                        members.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();
                    comboBoxALLMembers.DataSource = new BindingSource(members, null);
                    comboBoxALLMembers.DisplayMember = "Value";
                    comboBoxALLMembers.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SubmitAppointment();
        }
        private void SubmitAppointment()
        {
            if (comboBoxALLMembers.SelectedValue == null || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please complete all fields before submitting.");
                return;
            }

            int memberId = Convert.ToInt32(comboBoxALLMembers.SelectedValue);
            string appointment = textBox5.Text;
            string duration = textBox4.Text;

            InsertAppointment(memberId, appointment, duration);
        }

        private void InsertAppointment(int memberId, string appointment, string duration)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string checkQuery = "SELECT COUNT(*) FROM TrainerSession WHERE MemberID = @MemberID AND TrainerID = @TrainerID";
            string insertQuery = "INSERT INTO TrainerSession (TrainerID, MemberID, Duration, Appointmentstatus) VALUES (@TrainerID, @MemberID, @Duration, @Appointmentstatus)";
            string updateQuery = "UPDATE TrainerSession SET Duration = @Duration, Appointmentstatus = @Appointmentstatus WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@TrainerID", SessionManager.TrainerId);

                        int exists = (int)cmd.ExecuteScalar();
                        cmd.CommandText = exists > 0 ? updateQuery : insertQuery;
                        cmd.Parameters.AddWithValue("@Duration", duration);
                        cmd.Parameters.AddWithValue("@Appointmentstatus", appointment);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show(exists > 0 ? "Appointment updated successfully." : "Appointment created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to set or update the appointment: " + ex.Message);
            }
        }

    }
}
