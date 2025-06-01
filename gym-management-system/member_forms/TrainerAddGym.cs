using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static Member_Forms.Trainer_Login;
using System.Data.SqlClient;
namespace Member_Forms
{
    public partial class TrainerAddGym : Form
    {
        public TrainerAddGym()
        {
            InitializeComponent();
        }

        private void TrainerAddGym_Load(object sender, EventArgs e)
        {
            LoadAvailableGyms(SessionManager.TrainerId);
        }

        private void LoadAvailableGyms(int trainerId)
        {
            string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";
            string query = @"
    SELECT GYM.GYMID, GYM.GYMNAME 
    FROM GYM
    LEFT JOIN TrainerGetAddGym ON GYM.GYMID = TrainerGetAddGym.GymID AND TrainerGetAddGym.TrainerID = @TrainerID
    WHERE TrainerGetAddGym.GymID IS NULL";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<KeyValuePair<int, string>> gyms = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["GYMID"].ToString());
                        string name = reader["GYMNAME"].ToString();
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
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrainerReqToOwnertoAdd();
        }

        private void TrainerReqToOwnertoAdd()
        {
            if (comboBoxGyms.SelectedItem != null)
            {
                string selectedGym = comboBoxGyms.SelectedItem.ToString();
                var match = Regex.Match(selectedGym, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int selectedGymId))
                {
                    int trainerId = SessionManager.TrainerId;
                    string connectionString = "Data Source=DESKTOP-T91EAJS\\SQLEXPRESS;Initial Catalog=DB_Project;Integrated Security=True;Encrypt=False";

                    string checkQuery = "SELECT COUNT(*) FROM TrainerGetAddGym WHERE TrainerID = @TrainerID AND GymID = @GymID";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@TrainerID", trainerId);
                        checkCmd.Parameters.AddWithValue("@GymID", selectedGymId);
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("You have already requested to request this gym.");
                            return;
                        }

                        string query = "INSERT INTO TrainerGetAddGym (TrainerID, GymID) VALUES (@TrainerID, @GymID)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@TrainerID", trainerId);
                            cmd.Parameters.AddWithValue("@GymID", selectedGymId);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Request sent to the gym owner.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to parse gym ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a gym to add.");
            }
        }

    }
}
