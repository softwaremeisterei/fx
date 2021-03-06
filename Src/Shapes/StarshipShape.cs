using System;

/// <summary>
/// Summary description for Starship.
/// </summary>
public class StarshipShape : Shape
{
    public double Height = 1.618;
    public double Width = 1;
    double a;
    private double b;

    public StarshipShape()
    {
        a = Math.Atan(this.Width / 2 / this.Height);
        b = Math.Sqrt(this.Width * this.Width / 4 + this.Height * this.Height);
    }

    public override void Paint(System.Drawing.Graphics g, double x, double y, double scale)
    {
        Vector pos = new Vector(x, y);
        Vector orientation = this.Orientation.Copy();

        Turtle turtle = new Turtle(g, pos, orientation);
        turtle.SetColor(System.Drawing.Color.Red);
        turtle.Backward(this.Height / 2 * scale);
        turtle.SetColor(System.Drawing.Color.Black);

        turtle.Save();
        turtle.Left(NaturalConstants.Degree90);
        turtle.Forward(this.Width / 2 * scale);
        turtle.Right(a + NaturalConstants.Degree90);
        turtle.Forward(b * scale);

        turtle.Recall();
        turtle.Right(NaturalConstants.Degree90);
        turtle.Forward(this.Width / 2 * scale);
        turtle.Left(a + NaturalConstants.Degree90);
        turtle.Forward(b * scale);
    }
}
