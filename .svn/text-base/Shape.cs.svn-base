using System;

namespace fx
{
	/// <summary>
	/// Summary description for Shape.
	/// </summary>
	public class Shape
	{
		private Vector p_Orientation = Vector.UnitVectorX;
		private System.Drawing.Pen p_Pen = System.Drawing.Pens.Black;

		public Vector Orientation 
		{
			get { return this.p_Orientation; }
			set { this.p_Orientation = value; }
		}

		public System.Drawing.Pen Pen 
		{
			get { return this.p_Pen; }
			set { this.p_Pen = value; }
		}

		public Shape()
		{
		}

		public Shape(System.Drawing.Pen pen)
		{
			this.Pen = pen;
		}


		public virtual void Paint(System.Drawing.Graphics g, double x, double y, double scale) 
		{
		}
	}
}
