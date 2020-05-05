using System;

/// <summary>
/// Summary description for Circle.
/// </summary>
public class CircleShape : Shape
{
    public double Radius { get; set; } = 1;

    public CircleShape()
        : base()
    {
    }

    public CircleShape(double radius)
        : this()
    {
        this.Radius = radius;
    }

    public CircleShape(System.Drawing.Pen pen)
        : base(pen)
    {
    }

    public CircleShape(System.Drawing.Pen pen, double radius)
        : base(pen)
    {
        this.Radius = radius;
    }

    public override void Paint(System.Drawing.Graphics g, double x, double y, double scale)
    {
        double scaledRadius = this.Radius * scale;
        if ((x + scaledRadius < g.VisibleClipBounds.Left || x - scaledRadius > g.VisibleClipBounds.Right)
                &&
            (y + scaledRadius < g.VisibleClipBounds.Top || y - scaledRadius < g.VisibleClipBounds.Bottom))
            return;

        Vector orientation = this.Orientation.Copy();

        if (this.Radius == 0)
        {
            g.DrawLine(this.Pen, (float)x, (float)y, (float)(x + 1), (float)(y));
        }
        else
        {
            scaledRadius = Math.Max(Math.Ceiling(scaledRadius), 0.1);
            g.DrawEllipse(this.Pen,
                (float)(x - scaledRadius),
                (float)(y - scaledRadius),
                (float)scaledRadius * 2,
                (float)scaledRadius * 2);

            orientation *= scaledRadius;
            float x0 = (float)(x + (orientation * 0.9).X);
            float y0 = (float)(y + (orientation * 0.9).Y);
            float x1 = (float)(x + (orientation * 1.2).X);
            float y1 = (float)(y + (orientation * 1.2).Y);

            g.DrawLine(this.Pen, x0, y0, x1, y1);
        }
    }
}
