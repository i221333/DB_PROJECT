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

namespace Member_Forms
{
    public partial class TrainerMemberPlans : Form
    {
        public TrainerMemberPlans()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrainerReports form = new TrainerReports(); 
            form.ShowDialog();  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TrainerMemberPlans_Load(object sender, EventArgs e)
        {
            LoadTrainers();
        }

        private void LoadTrainers()
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = "SELECT ID, FIRSTNAME FROM Trainer";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> trainers = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        trainers.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }

                    comboBoxALLTrainers.DataSource = new BindingSource(trainers, null);
                    comboBoxALLTrainers.DisplayMember = "Value";
                    comboBoxALLTrainers.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load trainers: " + ex.Message);
            }
        }

        private void comboBoxALLTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxALLTrainers.SelectedValue != null)
            {
                int selectedTrainerId = ((KeyValuePair<int, string>)comboBoxALLTrainers.SelectedItem).Key;
                LoadMembers(selectedTrainerId);
            }
        }

        private void LoadMembers(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"SELECT m.ID, m.FIRSTNAME FROM Member m
                           JOIN TrainerMembers tm ON m.ID = tm.MemberID 
                           JOIN Trainer t ON tm.TrainerID = t.ID
                           WHERE t.ID = @TrainerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    //cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    cmd.Parameters.Add("@TrainerID", SqlDbType.Int).Value = trainerId;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> members = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        members.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }

                    comboBoxALLMembers.DataSource = new BindingSource(members, null);
                    comboBoxALLMembers.DisplayMember = "Value";
                    comboBoxALLMembers.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load members: " + ex.Message);
            }
        }

        private void comboBoxALLMembers_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxALLMembers.SelectedValue != null)
            {
                ClearAllTextBoxes();
                int selectedMemberId = ((KeyValuePair<int, string>)comboBoxALLMembers.SelectedItem).Key;
                FetchAndDisplayDietAndExercisePlans(selectedMemberId);
            }
        }

        private void ClearAllTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
        }

        private void FetchAndDisplayDietAndExercisePlans(int memberId)
        {
            FetchAndDisplayDietPlan(memberId);
            FetchAndDisplayExercisePlan(memberId);
        }

        private void FetchAndDisplayDietPlan(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
        SELECT TypeOfDiet, Nutrition, Purpose, Peanuts, Gluten, Lactose, Carbs, Protein, Fat, Fibre 
        FROM MemberDietPlan 
        WHERE MemberID = @MemberID";

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
                       
                        textBox2.Text = reader["TypeOfDiet"].ToString();
                        textBox3.Text = reader["Nutrition"].ToString();
                        textBox4.Text = reader["Purpose"].ToString();
                        textBox5.Text = reader["Peanuts"].ToString();
                        textBox6.Text = reader["Gluten"].ToString();
                        textBox7.Text = reader["Lactose"].ToString();
                        textBox8.Text = reader["Carbs"].ToString();
                        textBox9.Text = reader["Protein"].ToString();
                        textBox10.Text = reader["Fat"].ToString();
                        textBox11.Text = reader["Fibre"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No diet plan found for this member.");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch diet plan: " + ex.Message);
            }
        }
        private void FetchAndDisplayExercisePlan(int memberId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
        SELECT ExerciseName, ESets, EReps, TargetMuscle, Machine, RestInterval 
        FROM MemberExercise 
        WHERE MemberID = @MemberID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // Assuming there is only one exercise plan per member
                    {
                        // Update the text boxes or labels with the fetched data
                        textBox16.Text = reader["ExerciseName"].ToString();
                        textBox15.Text = reader["ESets"].ToString();
                        textBox1.Text = reader["EReps"].ToString();
                        textBox12.Text = reader["TargetMuscle"].ToString();
                        textBox14.Text = reader["Machine"].ToString();
                        textBox13.Text = reader["RestInterval"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No exercise plan found for the selected member.");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch exercise plan: " + ex.Message);
            }
        }


    }
}
