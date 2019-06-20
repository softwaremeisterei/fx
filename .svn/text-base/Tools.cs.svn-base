using System;
using System.Drawing;

namespace fx.Tools
{
	/// <summary>
	/// Drawing tool based on logo's turtle graphic 
	/// </summary>
	public class Turtle
	{
		private Graphics p_Graphics = null;
		private double p_PenWidth = 1;
		private Pen p_Pen = Pens.Black;
		private bool p_PenDown = true;
		private Turtle p_Save = null;

		public Vector Position = Vector.NullVector;
		public double Angle = 0;

		/// <summary>
		/// Saves the actual state of the turtle. 
		/// The turtle can later be reset to this state by calling method Recall()
		/// </summary>
		public void Save() 
		{
			this.p_Save = new Turtle(this);
		}

		/// <summary>
		/// Resets the turtle to the state previously saved by method Save()
		/// </summary>
		public void Recall() 
		{
			if (this.p_Save == null) return;
			Turtle.Copy(this.p_Save, this);
		}

		/// <summary>
		/// Copy ctor
		/// </summary>
		/// <param name="o"></param>
		public Turtle(Turtle o) 
		{
			Turtle.Copy(o, this);
		}

		/// <summary>
		/// Copy the turtle state from one to the other turtle
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void Copy(Turtle from, Turtle to) 
		{
			to.p_Graphics = from.p_Graphics;
			to.p_Pen = from.p_Pen;
			to.p_PenDown = from.p_PenDown;
			to.p_Save = from.p_Save;
			to.Position = from.Position;
			to.Angle = from.Angle;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="g"></param>
		/// <param name="position">start position of the turtle</param>
		/// <param name="direction"></param>
		public Turtle(Graphics g, Vector position, Vector direction)
		{
			this.p_Graphics = g;
			this.Position = position.Copy();
			this.Angle = direction.Angle;
		}

		/// <summary>
		/// Puts the pen down, so subsequent movements of the turtle will draw lines
		/// </summary>
		public void PenDown() 
		{
			this.p_PenDown = true;
		}

		/// <summary>
		/// Lifts the pen, so movements of the turtle will not draw anything
		/// </summary>
		public void PenUp() 
		{
			this.p_PenDown = false;
		}

		/// <summary>
		/// Moves the turtle the given distance forward
		/// </summary>
		/// <param name="d"></param>
		public void Forward(double d) 
		{
			Vector p1 = this.Position + Vector.UnitVectorX.Rotate(this.Angle) * d;
			this.MoveTo(p1);
		}

		/// <summary>
		/// Moves the turtle the given distance backward
		/// </summary>
		/// <param name="d"></param>
		public void Backward(double d) 
		{
			Vector p1 = this.Position - Vector.UnitVectorX.Rotate(this.Angle) * d;
			this.MoveTo(p1);
		}

		/// <summary>
		/// Moves the turtle to the specified position
		/// </summary>
		/// <param name="p"></param>
		public void MoveTo(Vector p) 
		{
			if (this.p_PenDown) 
			{
				this.p_Graphics.DrawLine(this.p_Pen, 
					(float)this.Position.X, 
					(float)this.Position.Y, 
					(float)p.X, 
					(float)p.Y);
			}
			this.Position = p;
		}

		/// <summary>
		/// Turns the turtle the given angle left
		/// </summary>
		/// <param name="angle">the angle in radians</param>
		public void Left(double angle) 
		{
			this.Angle -= angle;
			//this.Angle %= CosmicConstants.Degree360;
		}

		/// <summary>
		/// Turns the turtle the given angle right
		/// </summary>
		/// <param name="angle">the angle in radians</param>
		public void Right(double angle) 
		{
			this.Angle += angle;
			//this.Angle %= CosmicConstants.Degree360;
		}

		/// <summary>
		/// Sets the turtle pens drawing color
		/// </summary>
		/// <param name="color"></param>
		public void SetColor(Color color)
		{
			this.p_Pen = new Pen(color, (float)this.p_PenWidth);
		}

		/// <summary>
		/// Sets the turtle pens line drawing width
		/// </summary>
		/// <param name="penWidth"></param>
		public void SetPenWidth(double penWidth) 
		{
			this.p_PenWidth = penWidth;
		}
	}
}
