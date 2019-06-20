using System;
using System.Collections;

namespace fx
{
	/// <summary>
	/// Summary description for PhysicObject.
	/// </summary>
	public class PhysicObject
	{
		public String Name = String.Empty;

		public Vector Position = new Vector(0, 0);
		public double Speed = 0;
		public Vector Direction = new Vector(1, 0);
		public double Mass = 1.0F;
		public Vector GravForce = new Vector(0, 0);

		public Shape Shape = new CircleShape();
		public ArrayList Engines = new ArrayList(); // e.g. engines

		public DateTime LiveUntil = DateTime.MaxValue;

		public Vector Velocity 
		{
			get 
			{
				return this.Direction * this.Speed;
			}
			set 
			{
				this.Speed = value.Magnitude;
				if (value.Magnitude != 0) 
				{
					this.Direction = value.UnitVector;
				}
			}
		}

		public PhysicObject()
		{
		}

		public PhysicObject(String name)
		{
			this.Name = name;
		}

		public PhysicObject(String name, Vector position, Double speed, Vector direction, Double mass)
			 : this(name)
		{
			this.Position = position;
			this.Speed = speed;
			this.Direction = direction;
			this.Mass = mass;
		}

		public void MoveTo(Vector v) {
			this.Position = v;
		}
	}
}
