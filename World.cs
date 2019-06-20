using System;
using System.Collections.Generic;

namespace fx
{
    /// <summary>
    /// Summary description for World.
    /// </summary>
    public class World
    {
        /// <summary>
        /// ´The list of existing objects in this world
        /// </summary>
        public List<PhysicObject> PhysicObjects = new List<PhysicObject>();

        public World()
        {
        }

        /// <summary>
        /// Find an object by name
        /// </summary>
        /// <param name="name">the name of the object to find</param>
        /// <returns>the object with the given name or null</returns>
        public PhysicObject FindPhysicObject(String name)
        {
            foreach (PhysicObject o in this.PhysicObjects)
            {
                if (String.Compare(o.Name, name, true) == 0)
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

            PhysicObject m = new PhysicObject();
            m.Name = "Sun";
            m.Position = new Vector(0, 0);
            m.Mass = CosmicConstants.SolarSystem.Mass_Sun;
            m.Shape = new CircleShape(CosmicConstants.SolarSystem.Diameter_Sun);
            this.PhysicObjects.Add(m);

            // Earth

            m = new PhysicObject();
            m.Name = "Earth";
            m.Position = new Vector(CosmicConstants.AU, 0);
            m.Direction = new Vector(0, 1);
            m.Speed = CosmicConstants.SolarSystem.Velocity_Orbital_Mean_Earth;
            m.Mass = CosmicConstants.SolarSystem.Mass_Earth;
            m.Shape = new CircleShape(CosmicConstants.SolarSystem.Diameter_Earth);
            this.PhysicObjects.Add(m);

            //...

        }

        /// <summary>
        /// Garbace collection, removes dead objects from this world
        /// </summary>
        public void GC()
        {
            lock (this.PhysicObjects)
            {
                // todo: move this into another thread which does more general world management tasks
                DateTime now = DateTime.Now;
                bool done = false;
                while (!done)
                {
                    done = true;
                    foreach (PhysicObject o in this.PhysicObjects)
                    {
                        if (o.LiveUntil <= now)
                        {
                            this.PhysicObjects.Remove(o);
                            done = false;
                            break;
                        }
                    }
                }
            }
        }

    }
}
