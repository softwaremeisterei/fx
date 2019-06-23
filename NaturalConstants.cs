using System;
using System.Security.Cryptography.X509Certificates;

public class Conversions
{
    public static double DegreeToRad(int degree)
    {
        return degree * NaturalConstants.PI / 180;
    }
}

public class NaturalConstants
{

    public static readonly double PI = Math.PI;
    public static readonly double Degree90 = PI / 2;
    public static readonly double Degree180 = PI;
    public static readonly double Degree360 = PI * 2;

    public static readonly double Minute = 60;
    public static readonly double Hour = 60 * Minute;
    public static readonly double Day = 24 * Hour;
    public static readonly double Year = 365.2425 * Day;

    public static readonly double Month = Year / 12;

    public static readonly double Mio = 1E+6;
    public static readonly double km = 1E+3;

    /// <summary>
    /// astronomical unit [m]
    /// </summary>
    public static readonly double AU = 149.597870691 * Mio * km;
    /// <summary>
    /// gravitational constant [m³/kg/s²]
    /// </summary>
    public static readonly double G = 6.6742E-11;
    /// <summary>
    /// speed of light
    /// </summary>
    public static readonly double c = 2.99792458E+8;
    /// <summary>
    /// one day [s]
    /// </summary>
    public static readonly double d = 24 * 60 * 60;
    /// <summary>
    /// one year [s]
    /// </summary>
    public static readonly double a = 365.2425 * d;

    /// <summary>
    /// Facts about our solar system
    /// </summary>
    public class SolarSystem
    {

        //
        // Masses in kilograms											// http://www.unser-sonnensystem.info/pluto.html
        //

        public static readonly Double Mass_Earth = 5.98E24;
        public static readonly Double Mass_Moon = 7.35E22;
        public static readonly Double Mass_Sun = 333000 * Mass_Earth;
        public static readonly Double Mass_Merkur = 0.055 * Mass_Earth;
        public static readonly Double Mass_Venus = 0.815 * Mass_Earth;
        public static readonly Double Mass_Mars = 0.107 * Mass_Earth;
        public static readonly Double Mass_Jupiter = 318 * Mass_Earth;
        public static readonly Double Mass_Saturn = 95.14 * Mass_Earth;
        public static readonly Double Mass_Uranus = 14.5 * Mass_Earth;
        public static readonly Double Mass_Neptun = 17.2 * Mass_Earth;
        public static readonly Double Mass_Pluto = 0.0017 * Mass_Earth;

        //
        // Diameters in
        //

        public static readonly Double Diameter_Sun = 1.4 * Mio * km;
        public static readonly Double Diameter_Earth = 12753 * km;

        //
        // Velocity meters / sec
        //

        public static readonly Double Velocity_Orbital_Mean_Earth = 29.79 * km;

        //
        // Distances in meters
        //

        public static readonly Double Distance_Sun_Earth = NaturalConstants.AU;  // http://www.windows.ucar.edu/tour/link=/earth/statistics.html

        /// <summary>
        /// earth acceleration [m/s²]
        /// </summary>
        public static readonly double g = 9.80665;
    }
}
