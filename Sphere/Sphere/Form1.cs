using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace SphereProject
{
    public partial class Form1 : Form
    {
        Device d3d;
        Mesh box;
        Material boxMaterial;
        float time, angleFi, angleFrame;
        public static float radius, Mrot, M, Jrot, Jwh, fi0, fiSht0, mass,I,F;
        bool checkWF = false;

        public Form1()
        {
            InitializeComponent();
            d3d = null;
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public void OnIdle(object sender, EventArgs e)
        {
            Invalidate(); // Помечаем главное окно (this) как требующее перерисовки
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Устанавливаем режим отображения трехмерной графики
                PresentParameters d3dpp = new PresentParameters();
                d3dpp.BackBufferCount = 1;
                d3dpp.SwapEffect = SwapEffect.Discard;
                d3dpp.Windowed = true; // Выводим графику в окно
                d3dpp.MultiSample = MultiSampleType.None; // Выключаем антиалиасинг
                d3dpp.EnableAutoDepthStencil = true; // Разрешаем создание z-буфера
                d3dpp.AutoDepthStencilFormat = DepthFormat.D16; // Z-буфер в 16 бит
                d3d = new Device(0, // D3D_ADAPTER_DEFAULT - видеоадаптер по 
                    // умолчанию
                      DeviceType.Hardware, // Тип устройства - аппаратный ускоритель
                      this, // Окно для вывода графики
                      CreateFlags.SoftwareVertexProcessing, // Геометрию обрабатывает CPU
                      d3dpp);

            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Ошибка инициализации");
                Close(); // Закрываем окно
            }
            box = Mesh.Box(d3d, 1, 1, 1);
            boxMaterial = new Material();
            boxMaterial.Diffuse = Color.Black;
            boxMaterial.Specular = Color.White;


            ChangeData();
            //changeLables();
            //////////!!!!!!!!!!!!!!!!!!!!
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            d3d.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Azure, 1.0f, 0);
            d3d.BeginScene();
            SetupProekcii();


            float L1 = 20.0f;
            float M1 = 10.0f;
            
            Draw(L1, M1);

            d3d.EndScene();
            //Показываем содержимое дублирующего буфера
            d3d.Present();
        }

        private void SetupProekcii()
        {
            d3d.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2, this.Width / this.Height, 1.0f, 50.0f);
            d3d.Lights[0].Enabled = true;   // Включаем нулевой источник освещения
            d3d.Lights[0].Diffuse = Color.White;         // Цвет источника освещения
            d3d.Lights[0].Position = new Vector3(0, 0, 0); // Задаем координаты
            //d3d.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1.0f, 50.0f);
        }

        private void Draw(float L1, float M1)
        {
            
            
            /*
             *     __________
             *  O-|          |-O
             *    |          |
             *    |          |
             *    |          |<----- L1
             *    |      __  |
             *    |   | |  | |
             *  O- ___|______ -O
                       ^
             *         |
             *         M1
             *         
             * 
             * 
             * 
             */

            // КОЛЕСА
            Mesh wheel = Mesh.Cylinder(d3d, (float)numericOmega.Value, (float)numericOmega.Value, 0.5f, 50, 50);
            Material wheelMaterial = new Material();
            wheelMaterial.Diffuse = Color.Brown;
            wheelMaterial.Specular = Color.White;
            // ПРОДОЛЬНЫЙ СТЕРЖЕНЬ
            Mesh rod1 = Mesh.Cylinder(d3d, 0.2f, 0.2f, M1 + 1.0f, 10, 10);
            Material rod1Material = new Material();
            rod1Material.Diffuse = Color.Black;
            rod1Material.Specular = Color.White;
            // ПОПЕРЕЧНЫЙ СТЕРЖЕНЬ
            Mesh rod2 = Mesh.Cylinder(d3d, 0.2f, 0.2f, L1 - 1.0f, 10, 10);
            Material rod2Material = new Material();
            rod2Material.Diffuse = Color.Black;
            rod2Material.Specular = Color.White;
            // КОЛЕСА РЕДУКТОРА
            Mesh red = Mesh.Cylinder(d3d, 1.5f, 1.5f, 0.5f, 50, 50);
            Material red1Material = new Material();
            red1Material.Diffuse = Color.Green;
            red1Material.Specular = Color.White;
            Material red2Material = new Material();
            red2Material.Diffuse = Color.Blue;
            red2Material.Specular = Color.White;
            // СТЕРЖЕНЬ ДВИГАТЕЛЯ
            Mesh dvS = Mesh.Cylinder(d3d, 0.2f, 0.2f, 2.0f, 10, 10);
            Material dvSMaterial = new Material();
            dvSMaterial.Diffuse = Color.Black;
            dvSMaterial.Specular = Color.White;
            // ДВИГАТЕЛЬ
            Mesh dv = Mesh.Cylinder(d3d, 1.0f, 1.0f, 3.0f, 10, 10);
            Material dvMaterial = new Material();
            dvMaterial.Diffuse = Color.Red;
            dvMaterial.Specular = Color.White;

            // Вращение и отдаление скролл барами
            //  Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX((float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            
            /////////// КОЛЕСА
            if (checkWF)
                d3d.RenderState.FillMode = FillMode.WireFrame;
            else
                d3d.RenderState.FillMode = FillMode.Solid;

            d3d.Material = wheelMaterial;
            d3d.Transform.World = Matrix.RotationZ((float)(time * y1s / radius)) * Matrix.Translation(L1 / 2.0f, 0, M1 / 2.0f) * Matrix.Translation(-(float)y0_1+1.0f*L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            wheel.DrawSubset(0);
            d3d.Transform.World = Matrix.RotationZ((float)(time * y1s / radius)) * Matrix.Translation(-L1 / 2.0f, 0, M1 / 2.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            wheel.DrawSubset(0);
            d3d.Transform.World = Matrix.RotationZ((float)(time * y1s / radius)) * Matrix.Translation(L1 / 2.0f, 0, -M1 / 2.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            wheel.DrawSubset(0);
            d3d.Transform.World = Matrix.RotationZ((float)(time * y1s / radius)) * Matrix.Translation(-L1 / 2.0f, 0, -M1 / 2.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            wheel.DrawSubset(0);
            ////////// ПЕРЕКЛАДИНЫ
            d3d.RenderState.FillMode = FillMode.Solid;
            d3d.Material = rod1Material;
            d3d.Transform.World = Matrix.Translation(L1 / 2.0f, 0, 0) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            rod1.DrawSubset(0);
            d3d.Transform.World = Matrix.Translation(-L1 / 2.0f, 0, 0) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            rod1.DrawSubset(0);
            d3d.Material = rod2Material;
            d3d.Transform.World = Matrix.RotationY((float)Math.PI / 2.0f) * Matrix.Translation(0, 0, M1 / 2.0f - 1.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            rod2.DrawSubset(0);
            d3d.Transform.World = Matrix.RotationY((float)Math.PI / 2.0f) * Matrix.Translation(0, 0, -M1 / 2.0f + 1.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            rod2.DrawSubset(0);
            //////// РЕДУКТОР
            if (checkWF)
                d3d.RenderState.FillMode = FillMode.WireFrame;
            else
                d3d.RenderState.FillMode = FillMode.Solid;

            d3d.Material = red1Material;
            d3d.Transform.World = Matrix.RotationZ((float)(time * y1s / radius)) * Matrix.Translation(L1 / 2.0f, 0, 0) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            red.DrawSubset(0);
            d3d.Material = red2Material;
            d3d.Transform.World = Matrix.RotationZ(-(float)(time * y1s / (radius * I))) * Matrix.Translation(L1 / 2.0f, 3.0f, 0) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            red.DrawSubset(0);
            /////// ДВИГАТЕЛЬ
            d3d.Material = dvSMaterial;
            d3d.Transform.World = Matrix.RotationZ(-(float)(time * y1s / (radius * I))) * Matrix.Translation(L1 / 2.0f, 3.0f, 1.0f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            dvS.DrawSubset(0);
            d3d.Material = dvMaterial;
            d3d.Transform.World = Matrix.Translation(L1 / 2.0f, 3.0f, 2.5f) * Matrix.Translation(-(float)y0_1 + 1.0f * L1, 10.0f, 0) * Matrix.RotationY((float)hScrollBar2.Value / 10.0f) * Matrix.RotationX(-(float)vScrollBar1.Value / 10.0f) * Matrix.Translation(0, 0, (float)hScrollBar1.Value);
            dv.DrawSubset(0);

        }


        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }
                                             
        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (buttonStartStop.Text == "Start")
            {
                buttonStartStop.Text = "Stop";
                panel1.Enabled = false;
                timer1.Start();
            }
            else
            {
                buttonStartStop.Text = "Start";
                panel1.Enabled = true;
                timer1.Stop();
            }
        }

        double y0_1 = fi0, y0s = fiSht0,
             y1_1 = fi0, y1s = fiSht0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //time += omega / 5.0f;   //0.01
            time += 0.01f;
            //angleFrame += radius * 0.01f;

            RungeKutt.rungekuttIV(0.01f, y0_1, y0s, ref y1_1, ref y1s);
            y0_1 = y1_1;
            y0s = y1s;
            //angleFi = (float)y1_1;

            //changeLables();
            this.Invalidate();
        }
        private void ChangeData()
        {
            Mrot = (float)numericA.Value;
            M = (float)numericB.Value;
            Jrot = (float)numericC.Value;
            Jwh = (float)numericL.Value;
            radius = (float)numericOmega.Value;
            fi0 = (float)numericFi0.Value * (float)(Math.PI / 180.0f);
            fiSht0 = (float)numericFiSht0.Value;
            mass = (float)numericMass.Value;
            I = (float)numericI.Value;
            F = (float)numericF.Value;
            y0_1 = fi0; y0s = fiSht0;
            angleFi = fi0;
            checkWF = checkBox1.Checked;
            //changeLables();
            this.Invalidate();
        }
        private void numericOmega_ValueChanged(object sender, EventArgs e)
        {
            ChangeData();
        }

      

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeData();
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }
    }
}
