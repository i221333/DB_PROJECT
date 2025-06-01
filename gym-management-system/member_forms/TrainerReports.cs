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
using static Member_Forms.Owner_Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Member_Forms
{
    public partial class TrainerReports : Form
    {
        private bool isFormLoading = true;

        public TrainerReports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner_Interface form = new Owner_Interface();
            form.ShowDialog();
        }


        private void TrainerReports_Load(object sender, EventArgs e)
        {
            LoadGyms();
            comboBoxALLMembers.DataSource = null;
            comboBoxALLTrainers.DataSource = null;
            textBox5.Clear();
            textBox4.Clear();
            isFormLoading = false;

        }
    
        private void LoadGyms()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            int ownerId = SessionManager.OwnerId;
            string query = "SELECT GYMID, GYMNAME FROM GYM WHERE OWNERID = '"+ ownerId + "'";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                   
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OWNERID", ownerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> gyms = new List<KeyValuePair<int, string>>();
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No gyms found for the owner with ID: " + ownerId);
                    }

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        gyms.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                    }
                    reader.Close();

                    comboBoxGyms.DataSource = new BindingSource(gyms, null);
                    comboBoxGyms.DisplayMember = "Value";
                    comboBoxGyms.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load gyms: " + ex.Message);
            }
        }

        private void comboBoxGyms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGyms.SelectedValue != null)
            {
                comboBoxALLMembers.DataSource = null;
                comboBoxALLTrainers.DataSource = null;
                textBox5.Clear();
                textBox4.Clear();
                int selectedGymId = ((KeyValuePair<int, string>)comboBoxGyms.SelectedItem).Key;
                LoadTrainersForGym(selectedGymId);
            }
        }
        private void LoadTrainersForGym(int gymId)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False");
            try
            {
                conn.Open();
                string query = @"
            SELECT Trainer.ID, Trainer.FIRSTNAME
            FROM Trainer
            JOIN GymTrainers ON Trainer.ID = GymTrainers.TrainerID
            WHERE GymTrainers.GymID = @GymID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GymID", gymId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<KeyValuePair<int, string>> trainers = new List<KeyValuePair<int, string>>();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    trainers.Add(new KeyValuePair<int, string>(id, $"{id} - {name}"));
                }
                comboBoxALLTrainers.DataSource = new BindingSource(trainers, null);
                comboBoxALLTrainers.DisplayMember = "Value";
                comboBoxALLTrainers.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load trainers: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
  
        private void comboBoxALLTrainers_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxALLTrainers.SelectedItem != null)
            {
                comboBoxALLMembers.DataSource = null;
                textBox5.Clear();
                textBox4.Clear();
                var selectedTrainer = (KeyValuePair<int, string>)comboBoxALLTrainers.SelectedItem;
                int selectedTrainerId = selectedTrainer.Key;
                LoadMembers(selectedTrainerId);
            }
        }

        private void LoadMembers(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
                SELECT Member.ID, Member.FIRSTNAME 
                FROM Member
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

        private void comboBoxALLMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Clear();
            textBox4.Clear();
            FetchAndDisplayReport();
        }

        private void FetchAndDisplayReport()
        {
            if (isFormLoading) return;

            if (comboBoxALLTrainers.SelectedValue != null && comboBoxALLMembers.SelectedValue != null)
            {
                var selectedTrainer = (KeyValuePair<int, string>)comboBoxALLTrainers.SelectedItem;
                int selectedTrainerId = selectedTrainer.Key;

                var selectedMember = (KeyValuePair<int, string>)comboBoxALLMembers.SelectedItem;
                int selectedMemberId = selectedMember.Key;

                string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
                string query = @"SELECT Rating, Comment FROM MemberFeedback WHERE MemberID = @MemberID AND TrainerID = @TrainerID";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MemberID", selectedMemberId);
                        cmd.Parameters.AddWithValue("@TrainerID", selectedTrainerId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            textBox5.Text = reader["Rating"].ToString();
                            textBox4.Text = reader["Comment"].ToString();
                        }
                        else
                        {
                            textBox5.Clear();
                            textBox4.Clear();
                            MessageBox.Show("No Report found for this member from the selected trainer.");
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load Report: " + ex.Message);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            TrainerMemberPlans form = new TrainerMemberPlans();
            form.ShowDialog(); 
        }
    }
}