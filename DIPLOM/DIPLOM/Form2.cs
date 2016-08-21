using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIPLOM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

    
        private void label14_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
           
            
            }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label8.Text=(Convert.ToString(trackBar1.Value))+("кг");            
            Form1.M1=trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label9.Text=(Convert.ToString(trackBar2.Value))+("кг");
            Form1.M2 = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label10.Text=(Convert.ToString(trackBar3.Value))+("см");
            Form1.R1 = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label11.Text=(Convert.ToString(trackBar4.Value))+("см");
            Form1.R2 = trackBar4.Value;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label12.Text = (Convert.ToString(trackBar5.Value)) + ("см");
            Form1.L1 = trackBar5.Value;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            label13.Text = (Convert.ToString(trackBar6.Value));
            Form1.Teta0 = trackBar6.Value;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            label14.Text = (Convert.ToString(trackBar7.Value));
            Form1.Fi0 = trackBar7.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar3.Value < trackBar4.Value )
                MessageBox.Show("Радиус дуги должен быть больше радиуса диска");
            else
            {              
                Form1.flag = true;
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
