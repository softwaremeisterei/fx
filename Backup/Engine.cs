using System;
using System.Collections;

namespace fx
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine
	{
		public DateTime LastUpdate = DateTime.Now;

		public Vector Force = Vector.NullVector;

		public double MaxStrength = 1;

		public double VariationPerSecond = 1;

		public Engine()
		{
		}

		public Engine(Vector initialForce, double variationPerSecond)
		{
			this.Force = initialForce;
			this.VariationPerSecond = variationPerSecond;
		}

		public void OnTimeElapsed(double dtms) 
		{
			this.Force *= VariationPerSecond * dtms / 1000;
		}
	}
}
