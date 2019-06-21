using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx
{
    public class CoarseTrigonometry
    {
        private double[] sinus = new double[360];
        private double[] cosinus = new double[360];
        private double[] tangens = new double[360];
        private Dictionary<double, double> atangens = new Dictionary<double, double>();

        public CoarseTrigonometry()
        {
            for (var i = 0; i < 360; i++)
            {
                sinus[i] = Math.Sin(i * Math.PI / 180);
                cosinus[i] = Math.Cos(i * Math.PI / 180);
                tangens[i] = Math.Tan(i * Math.PI / 180);
                atangens[i] = Math.Atan(i * Math.PI / 180);
            }
        }

        public double Sin(int deg)
        {
            if (deg < 0) deg = 360 - ((-deg) % 360);
            return sinus[(deg << 0) % 360];
        }

        public double Cos(int deg)
        {
            if (deg < 0) deg = 360 - ((-deg) % 360);
            return cosinus[(deg << 0) % 360];
        }

        public double Tan(int deg)
        {
            if (deg < 0) deg = 360 - ((-deg) % 360);
            return tangens[(deg << 0) % 360];
        }

        public double Atan(double dx, double dy)
        {
            var frac = dx / dy;
            frac = Math.Round(frac, 3);

            if (!atangens.TryGetValue(frac, out double result))
            {
                result = Math.Atan(frac);
                atangens.Add(frac, result);
            }

            return result;
        }
    }
}
