using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for PhysicObject.
/// </summary>
public class MassObject
{
    public string Name = string.Empty;

    public Vector Position = new Vector(0, 0);
    public Vector Direction = new Vector(1, 0);
    public Vector GravForce = new Vector(0, 0);

    public double Mass = 1.0F;
    public double Speed = 0;

    public Shape Shape = new CircleShape();

    public List<Engine> Engines = new List<Engine>(); // e.g. engines

    public DateTime LiveUntil = DateTime.MaxValue;

    public Vector Velocity
    {
        get { return Direction * Speed; }
        set
        {
            Speed = value.Magnitude;
            Direction = value.UnitVector;
        }
    }

    public MassObject()
    { }

    public MassObject(string name)
    {
        Name = name;
    }

    public MassObject(string name, Vector position, double speed, Vector direction, double mass)
         : this(name)
    {
        Position = position;
        Speed = speed;
        Direction = direction;
        Mass = mass;
    }

    public void MoveTo(Vector v)
    {
        Position = v;
    }
}
