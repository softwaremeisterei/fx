using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for World.
/// </summary>
public class World
{
    /// <summary>
    /// The list of existing objects in this world
    /// </summary>
    public List<MassObject> Objects = new List<MassObject>();

    public World()
    {
    }

    /// <summary>
    /// Find an object by name
    /// </summary>
    public MassObject FindObject(string name)
    {
        foreach (MassObject o in Objects)
        {
            if (string.Compare(o.Name, name, true) == 0)
            {
                return o;
            }
        }
        return null;
    }

    /// <summary>
    /// Adds the solar system's sun and planets to this world
    /// </summary>
    public void InsertSolarSystem()
    {
        // Sun

        MassObject m = new MassObject();
        m.Name = "Sun";
        m.Position = new Vector(0, 0);
        m.Mass = NaturalConstants.SolarSystem.Mass_Sun;
        m.Shape = new CircleShape(NaturalConstants.SolarSystem.Diameter_Sun);
        Objects.Add(m);

        // Earth

        m = new MassObject();
        m.Name = "Earth";
        m.Position = new Vector(NaturalConstants.AU, 0);
        m.Direction = new Vector(0, 1);
        m.Speed = NaturalConstants.SolarSystem.Velocity_Orbital_Mean_Earth;
        m.Mass = NaturalConstants.SolarSystem.Mass_Earth;
        m.Shape = new CircleShape(NaturalConstants.SolarSystem.Diameter_Earth);
        Objects.Add(m);

        //...
    }

    /// <summary>
    /// Garbace collection, removes dead objects from this world
    /// </summary>
    public void CollectGarbage()
    {
        lock (Objects)
        {
            // todo: move this into another thread which does more general world management tasks
            DateTime now = DateTime.Now;
            for (int index = Objects.Count - 1; index >= 0; index--)
            {
                var obj = Objects[index];

                if (obj.LiveUntil <= now)
                {
                    Objects.Remove(obj);
                }
            }
        }
    }

}
