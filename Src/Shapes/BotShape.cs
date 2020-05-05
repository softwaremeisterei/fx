using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BotShape : Shape
{
    public override void Paint(System.Drawing.Graphics g, double x, double y, double scale)
    {
        double a = 1.6 * scale;

        Vector pos = new Vector(x, y);
        Vector orientation = this.Orientation.Copy();

        var t = new Turtle(g, pos, orientation);

        t.Save();
        t.Left(Conversions.DegreeToRad(80));
        t.Forward(a/2);
        t.Right(Conversions.DegreeToRad(45));
        t.Forward(a/2);
        t.Right(Conversions.DegreeToRad(160));
        t.Forward(a*2/3);
        t.MoveTo(pos + orientation.UnitVector * 1.4 * a);

        t.Recall();
        t.Right(Conversions.DegreeToRad(80));
        t.Forward(a/2);
        t.Left(Conversions.DegreeToRad(45));
        t.Forward(a/2);
        t.Left(Conversions.DegreeToRad(160));
        t.Forward(a*2/3);
        t.MoveTo(pos + orientation.UnitVector * 1.4 * a);
    }
}
