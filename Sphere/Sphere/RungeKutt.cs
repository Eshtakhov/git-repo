using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphereProject
{
    class RungeKutt
    {
        public static void rungekuttIV(double h, double y0, double y0s, ref double y1, ref double y1s)
        {
            double k1 = h * func(y0);
            double k2 = h * func(y0 + h / 2 * y0s + h / 8 * k1);
            double k3 = h * func(y0 + h / 2 * y0s + h / 8 * k2);
            double k4 = h * func(y0 + h * y0s + h / 2 * k3);
            y1s = y0s + ((k1 + 2 * k2 + 2 * k3 + k4) / 6);
            y1 = y0 + h * (y0s + (k1 + k2 + k3) / 6);
        }

        private static double func(double y0)
        {
            return ((Form1.Mrot - Form1.M) * Form1.I * Form1.radius - Form1.F) / (Form1.mass + (4 * Form1.Jwh / Math.Pow(Form1.radius, 2) + Form1.Jrot / (Math.Pow(Form1.I, 2) * Math.Pow(Form1.radius, 2))));
            
        }

    }
}
