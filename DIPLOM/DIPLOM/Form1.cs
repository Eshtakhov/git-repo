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
    public partial class Form1 : Form
    {
        static public bool flag;
       static public double M1 = 0;
        static public double M2=0;
       static public int R1 = 0; static public int R2 = 0; static public int L1 = 0;
        static public double Teta0 = 0, Fi0 = 0;

        public Form1()
        {
            InitializeComponent();
        }
        static bool flag1 = false;
        static public Point Center;
        public int r1 = 15, r2 = 5, L = 16;
        public double m1 = 2.0, m2 = 1.0, G=9.81,h=0.01;
        public double teta0 = Math.PI / 6, fi0 = Math.PI/4;
        static public double dteta0 = 0, dfi0 = 0,y1,dy1,y2,dy2;       
        Graphics g = null;
        private void Start()
        {
            Pen myPen = new Pen(Color.Black);
            SolidBrush SBr = new SolidBrush(Color.White);
            Pen p = new Pen(Color.Black);          
        }
        public void Param()
        {
          r1 = R1; r2 = R2; L = L1;
        m1 = M1; m2 = M2;
        teta0 =Math.PI/(180/Teta0); fi0 =Math.PI/(180/ Fi0);
        }

        private double funck1(double teta, double dteta, double fi, double dfi)
        {
            double a1, a2, b1, b2, c1, c2;
            a1 = (3.0 / 2 * m1 + m2) * Math.Pow(r1 - r2,2) ;
            b2 = m2 * Math.Pow(L,2);
            a2 = m2 * (r1 - r2) * L * Math.Cos(teta - fi);
            b1 = m2 * (r1 - r2) * L * Math.Cos(teta - fi);
            c1 = -(m1 + m2) * G * (r1 - r2) * Math.Sin(teta) - m2 * (r1 - r2) * L * Math.Sin(teta - fi) * Math.Pow(dfi,2);
            c2 = -m2 * G * L * Math.Sin(fi) + m2 * (r1 - r2) * L * Math.Sin(teta - fi) * Math.Pow(dteta,2);
            double f = ((c1 * b2) - (c2 * b1)) / ((a1 * b2) - (b1 * a2));
            return f;
        }
        private double funck2( double fi,  double dfi,double teta,double dteta)
        {
            double a1, a2, b1, b2, c1, c2;
            a1 = (3.0 / 2 * m1 + m2) * Math.Pow(r1 - r2, 2);
            b2 = m2 * Math.Pow(L, 2);
            a2 = m2 * (r1 - r2) * L * Math.Cos(teta - fi);
            b1 = m2 * (r1 - r2) * L * Math.Cos(teta - fi);
            c1 = (-1) * (m1 + m2) * G * (r1 - r2) * Math.Sin(teta) - m2 * (r1 - r2) * L * Math.Sin(teta - fi) * Math.Pow(dfi, 2);
            c2 = - m2 * G * L * Math.Sin(fi) + m2 * (r1 - r2) * L * Math.Sin(teta - fi) * Math.Pow(dteta, 2);
            double f = ((c1 * a2) - (c2 * a1)) / ((b1 * a2) - (a1 * b2));
            return f;
        }
        private void RK1(double h1, double y0, double dy0, double alfa, double dalfa)
        {
            double k1, k2, k3, k4;
            k1 = h1 * funck1(y0, dy0, alfa, dalfa);
            k2 = h1 * funck1(y0 + h1 * dy0 / 2 + h1 * k1 / 8, dy0 + k1 / 2, alfa, dalfa);
            k3 = h1 * funck1(y0 + h1 * dy0 / 2 + h1 * k2 / 8, dy0 + k2 / 2, alfa, dalfa);
            k4 = h1 * funck1(y0 + h1 * dy0 + h1 * k3 / 2, dy0 + k3, alfa, dalfa);
            y1 = y0 + h1 * (dy0 + (k1 + k2 + k3) / 6);
            dy1 = dy0 + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
        }

      private void RK2(double h1, double y0, double dy0, double alfa, double dalfa)
      {
          double k1, k2, k3, k4;
          k1 = h1 * funck2(y0, dy0, alfa, dalfa);
          k2 = h1 * funck2(y0 + h1 * dy0 / 2 + h1 * k1 / 8, dy0 + k1 / 2, alfa, dalfa);
          k3 = h1 * funck2(y0 + h1 * dy0 / 2 + h1 * k2 / 8, dy0 + k2 / 2, alfa, dalfa);
          k4 = h1 * funck2(y0 + h1 * dy0 + h1 * k3 / 2, dy0 + k3, alfa, dalfa);
          y2 = y0 + h1 * (dy0 + (k1 + k2 + k3) / 6);
          dy2 = dy0 + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
      }   

    private void Picture(bool ch)
        {
            Point O1 = new Point();
            O1.X = Convert.ToInt32(pictureBox1.Location.X + pictureBox1.Width / 2);
            O1.Y = Convert.ToInt32(pictureBox1.Location.Y + pictureBox1.Height / 4);
            int R1 = 5 * r1, R2 = r2 * 5, l = L * 5;
            Graphics g = pictureBox1.CreateGraphics();
            Pen myPen = new Pen(Color.Black);
            SolidBrush SBr = new SolidBrush(Color.White);
            Pen p = new Pen(ch ? Color.Black : Color.White);  
            Point O2 = new Point();            
            Pen wp=new Pen(Color.White);       
            g.DrawEllipse(p, O1.X-R1, O1.Y-R1, 2 * R1, 2 * R1);
            g.FillRectangle(SBr, O1.X - R1, O1.Y - R1, 2 * R1, R1);
            g.DrawRectangle(wp, O1.X - R1, O1.Y - R1, 2 * R1, R1);
            O2.X = Convert.ToInt32(O1.X + (R1 - R2) * Math.Sin(teta0));
            O2.Y = Convert.ToInt32(O1.Y + (R1 - R2) * Math.Cos(teta0));
            Rectangle Rect = new Rectangle(O2.X - R2, O2.Y - R2,  2 * R2, 2 * R2);
            g.FillEllipse(SBr, Rect);
            g.DrawEllipse(p, Rect);
            SolidBrush BBr = new SolidBrush(Color.Black);
            Point O3 = new Point();
            O3.X = Convert.ToInt32(O2.X + l * Math.Sin(fi0));
            O3.Y = Convert.ToInt32(O2.Y + l * Math.Cos(fi0));
            Rect.Location = new Point(O3.X - 1, O3.Y - 1);
            Rect.Size = new Size(7 * 1, 7 * 1);
            SolidBrush myBr = new SolidBrush(ch ? Color.Black: Color.White);
            g.FillEllipse(myBr, Rect);
            g.DrawEllipse(p, Rect);
            g.DrawLine(p, O2, O3);
            myBr.Dispose(); wp.Dispose();
            p.Dispose(); BBr.Dispose();
            SBr.Dispose();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(0,menuStrip1.ClientSize.Height);           
            Center.X = pictureBox1.Width / 2;
            Center.Y = pictureBox1.Height / 4;
            g = pictureBox1.CreateGraphics();
            нарисоватьToolStripMenuItem.Enabled = false;            
            стеретьToolStripMenuItem.Enabled = false;
            button1.Enabled = false;       
            button2.Enabled = false;     
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Picture(false);
       
          RK1(h,teta0,dteta0,fi0,dfi0);
          RK2(h, fi0, dfi0, teta0, dteta0);
          teta0 = y1; dteta0 = dy1;
          fi0 = y2; dfi0 = dy2;            
          Picture(true);

        }

        private void beginToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag1== true)
            timer1.Enabled = true;
            нарисоватьToolStripMenuItem.Enabled = false;
            стеретьToolStripMenuItem.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = true; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            нарисоватьToolStripMenuItem.Enabled = false;
            стеретьToolStripMenuItem.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = false; 
            timer1.Enabled = false;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void Ochis()
        {
            SolidBrush SBr = new SolidBrush(Color.White);
            g.FillRectangle(SBr, 0, 0, pictureBox1.ClientRectangle.Width,pictureBox1.ClientRectangle.Height);
            SBr.Dispose();
        }
        
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ochis();
            Form2 form2 = new Form2();
            form2.ShowDialog();
            нарисоватьToolStripMenuItem.Enabled = true;
            стеретьToolStripMenuItem.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false; 
        }

        private void стеретьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ochis();
            timer1.Enabled = false;
            flag1 = false; 
            нарисоватьToolStripMenuItem.Enabled = false;
            стеретьToolStripMenuItem.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false; 
        }

        private void нарисоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (flag == true)
            {
                Param();
                Picture(true);
                flag1 = true;
                нарисоватьToolStripMenuItem.Enabled = false;
                стеретьToolStripMenuItem.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = false; 
            }
            
        }
    }
}
