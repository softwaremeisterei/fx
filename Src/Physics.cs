using System;

public class Physics
{
    public Physics()
    { }

    /// <summary>
    /// Calculate gravitational forces on the given objects in a time unit of dtms milliseconds
    /// </summary>
    public void Calculate(MassObject[] os, double dtms)
    {
        for (int i = 0; i < os.Length; i++)
        {
            os[i].GravForce.X = 0;
            os[i].GravForce.Y = 0;
        }

        for (int i = 0; i < os.Length; i++)
        {
            for (int j = i + 1; j < os.Length; j++)
            {
                CalcForces(os[i], os[j], dtms);
            }
        }

        for (int i = 0; i < os.Length; i++)
        {
            Translate(os[i], dtms);
        }
    }

    /// <summary>
    /// Moves the given object by calculating the distance covered 
    /// in the time period given in milliseconds
    /// </summary>
    private void Translate(MassObject o, double dtms)
    {
        var force = o.GravForce;

        foreach (var engine in o.Engines)
        {
            force += engine.Force;
        }

        o.Velocity += force / o.Mass * dtms / 1000;
        o.Position += o.Velocity * dtms / 1000;
    }

    /// <summary>
    /// Calculates the forces exerted by the given objects on each other
    /// </summary>
    private void CalcForces(MassObject o1, MassObject o2, double dtms)
    {

        double distance = (o2.Position - o1.Position).Magnitude;

        if (distance < 1E-5) return;

        var force = (o2.Position - o1.Position) 
                    * (NaturalConstants.G * o1.Mass * o2.Mass)
                    / (distance * distance * distance);

        o1.GravForce += force;
        o2.GravForce -= force;
    }
}
