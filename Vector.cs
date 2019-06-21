using System;
using fx;

/// <summary>
/// Summary description for Vector.
/// </summary>
public class Vector
{
    static CoarseTrigonometry _math = new CoarseTrigonometry();

    public double X;
    public double Y;

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Vector Copy()
    {
        return new Vector(X, Y);
    }

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }

    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y);
    }

    public static Vector operator *(Vector v, double a)
    {
        return new Vector(v.X * a, v.Y * a);
    }

    public static Vector operator /(Vector v, double a)
    {
        return new Vector(v.X / a, v.Y / a);
    }

    public static Vector NullVector
    {
        get { return new Vector(0, 0); }
    }

    public static Vector UnitVectorX
    {
        get { return new Vector(1, 0); }
    }

    public double Magnitude
    {
        get
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }

    public Vector UnitVector
    {
        get
        {
            if (Magnitude == 0) return this;
            return this / Magnitude;
        }
    }

    public double Angle
    {
        get
        {
            if (Magnitude == 0) return 0;
            if (X > 0)
                if (Y > 0)
                    return _math.Atan(Y, X);
                else
                    return NaturalConstants.PI * 2 + _math.Atan(Y, X);
            else
                if (Y > 0)
                return NaturalConstants.PI / 2 - _math.Atan(X, Y);
            else
                return NaturalConstants.PI * 3 / 2 - _math.Atan(X, Y);
        }
    }

    public Vector Rotate(double angle)
    {
        int deg = (int) (angle * 180 / Math.PI);
        double sin = _math.Sin(deg);
        double cos = _math.Cos(deg);
        double x1 = X * cos - Y * sin;
        double y1 = Y * cos + X * sin;
        X = x1;
        Y = y1;
        return this;
    }
}
