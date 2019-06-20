using System;

namespace fx
{
	/// <summary>
	/// Summary description for Physics.
	/// </summary>
	public class Physics
	{
		/// <summary>
		/// Ctor
		/// </summary>
		public Physics()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Calculate gravitational forces on the given objects in a time unit of dtms milliseconds
		/// </summary>
		/// <param name="physicObjects"></param>
		/// <param name="dtms">time unit in milliseconds</param>
		public void Calculate(PhysicObject[] os, double dtms) 
		{
			for(int i=0; i < os.Length; i++)  
			{
				os[i].GravForce.X = 0;
				os[i].GravForce.Y = 0;
			}

			for(int i=0; i < os.Length; i++) 
			{
				for(int j=i+1; j < os.Length; j++) 
				{
					this.CalculateForces(os[i], os[j], dtms);
				}
			}
			
			for(int i=0; i < os.Length; i++) 
			{
				this.Translate(os[i], dtms);
			}
		}

		/// <summary>
		/// Moves the given object by calculating the distance covered 
		/// in the time period given in milliseconds
		/// </summary>
		/// <param name="o">the object to by moved</param>
		/// <param name="dtms">the time slice in milliseconds</param>
		private void Translate(PhysicObject o, double dtms) 
		{
			Vector forceSum = o.GravForce;
			foreach(Engine e in o.Engines) 
			{
				forceSum += e.Force;
			}
			o.Velocity += forceSum / o.Mass * dtms / 1000;
			o.Position += o.Velocity * dtms / 1000;
		}

		/// <summary>
		/// Calculates the forces exerted by the given objects on each other
		/// </summary>
		/// <param name="o1"></param>
		/// <param name="o2"></param>
		/// <param name="dtms"></param>
		private void CalculateForces(PhysicObject o1, PhysicObject o2, double dtms) {

			double r = (o2.Position - o1.Position).Magnitude;

			if (r < 1E-5) return;

			Vector F12 = (o2.Position - o1.Position) * (CosmicConstants.G * o1.Mass * o2.Mass) 
							/ (r*r*r);
			
			Vector F21 = F12 * -1;

			o1.GravForce += F12;
			o2.GravForce += F21;
		}
	}
}
