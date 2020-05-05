using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fx
{
    public class Radar
    {
        static Pen focusedPen = new Pen(Color.Yellow);
        static Pen othersPen = new Pen(Color.Red);
        static Pen radarBorderPen = new Pen(Color.FromArgb(170, 170, 255));

        public Vector Position { get; set; } = new Vector(20, 20);
        public int Radius { get; set; } = 20;
        public double RealRadius = 100;

        public void Paint(Graphics g, Vector centerObj, Vector[] objectVectors)
        {
            var focusedObj = centerObj;

            if (focusedObj != null)
            {
                if (objectVectors.Length > 0)
                {
                    g.DrawEllipse(radarBorderPen, (float)(Position.X - Radius), (float)(Position.Y - Radius), Radius * 2, Radius * 2);

                    foreach (var objVector in objectVectors)
                    {
                        var distVector = objVector - focusedObj;
                        var distance = distVector.Magnitude;
                        var angle = distVector.Angle;

                        distance = Math.Min(Radius, distance * Radius / RealRadius);
                        distVector.SetMagnitude(distance);
                        var location = (Position + distVector);

                        var pen = (objVector == centerObj ? focusedPen : othersPen);

                        g.DrawEllipse(pen,
                            (float)location.X, (float)location.Y,
                            2, 2);
                    }
                }
            }
        }
    }
}
