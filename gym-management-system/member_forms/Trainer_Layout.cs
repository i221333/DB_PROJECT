using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member_Forms
{
    public partial class Trainer_Layout : Form
    {
        public Trainer_Layout()
        {
            InitializeComponent();
            PictureBox pictureBox = new PictureBox();

            pictureBox.Location = new Point(550, 100);
            pictureBox.Size = new Size(250, 300);

            pictureBox.Image = Image.FromFile("C:\\Users\\PC\\Desktop\\Project Deliverable 2\\trainer_layout.jpg");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            Trainer_WP form = new Trainer_WP();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Trainer_DP form = new Trainer_DP();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Trainer_EP form= new Trainer_EP();
            form.ShowDialog(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Trainer_TS form = new Trainer_TS();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Trainer_FB form= new Trainer_FB();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddMember form = new AddMember();
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrainerAddGym form = new TrainerAddGym();
            form.ShowDialog();
        }

        private void Trainer_Layout_Load(object sender, EventArgs e)
        {

        }
    }
}
