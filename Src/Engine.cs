using System;
using System.Collections;

/// <summary>
/// Summary description for Engine.
/// </summary>
public class Engine
{
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    public Vector Force { get; set; } = Vector.NullVector;
    public double MaxStrength { get; set; } = 1;
    public double VariationPerSecond { get; set; } = 1;

    public Engine()
    { }

    public Engine(Vector initialForce, double variationPerSecond)
    {
        Force = initialForce;
        VariationPerSecond = variationPerSecond;
    }

    public void OnTimeElapsed(double dtms)
    {
        Force *= VariationPerSecond * dtms / 1000;
    }
}