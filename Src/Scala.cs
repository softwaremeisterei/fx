using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx
{
    public class Scala
    {
        public Size Size { get; set; }
        public double SpatialScale { get; set; }
        public Font Font { get; set; }

        public void Paint(Graphics g)
        {
            int x0 = Size.Width / 12;
            int y0 = Size.Height * 10 / 12;
            int x1 = Size.Width * 11 / 12;
            g.DrawLine(Pens.Gray, x0, y0, x1, y0);

            for (int i = 0; i < 11; i++)
            {
                int x = (int)(x0 + i * Size.Width / 12);
                g.DrawLine(Pens.Gray, x, y0 - 5, x, y0);
                g.DrawString((i * Size.Width / 12 / SpatialScale).ToString("F"), Font, Brushes.Gray, x, y0 + 5);
            }
        }
    }
}
