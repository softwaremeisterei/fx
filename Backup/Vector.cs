using System;

namespace fx
{
	/// <summary>
	/// Summary description for Vector.
	/// </summary>
	public class Vector
	{
		public double X;
		public double Y;
		
		public Vector(double X, double Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public Vector Copy() 
		{
			return new Vector(this.X, this.Y);
		}

		public static Vector operator+(Vector a, Vector b) {
			return new Vector(a.X + b.X, a.Y + b.Y);
		}

		public static Vector operator-(Vector a, Vector b) {
			return new Vector(a.X - b.X, a.Y - b.Y);
		}

		public static Vector operator*(Vector v, double a) {
			return new Vector(v.X * a, v.Y * a);
		}

		public static Vector operator/(Vector v, double a) {
			return new Vector(v.X / a, v.Y / a);
		}

		public static Vector NullVector
		{
			get { return new Vector(0,0); }
		}
		
		public static Vector UnitVectorX 
		{
			get { return new Vector(1,0); }
		}

		public double Magnitude {
			get {
				return Math.Sqrt(this.X * this.X + this.Y*this.Y);
			}
		}

		public Vector UnitVector {
			get {
				if (this.Magnitude == 0) return this;
				return this / this.Magnitude;
			}
		}

		public double Angle 
		{
			get 
			{ 
				if (this.Magnitude == 0) return 0;
				if (this.X > 0) 
					if (Y > 0)
						return Math.Atan(this.Y/this.X);
					else
						return CosmicConstants.PI * 2 + Math.Atan(this.Y/this.X);
				else 
					if (Y > 0)
						return CosmicConstants.PI / 2 - Math.Atan(this.X/this.Y);
					else
						return CosmicConstants.PI * 3 / 2 - Math.Atan(this.X/this.Y);
			}
		}

		public Vector Rotate(double angle) 
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			double x1 = this.X * cos - this.Y * sin;
			double y1 = this.Y * cos + this.X * sin;
			this.X = x1;
			this.Y = y1;
			return this;
		}
	}
}
