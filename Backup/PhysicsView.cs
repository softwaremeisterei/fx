
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace fx
{
    /// <summary>
    /// Summary description for PhysicsView.
    /// </summary>
    public class PhysicsView : System.Windows.Forms.UserControl
    {
        Int32 CountObjectsCalculationTreshold = 200;

        Random p_Randomizer = new Random();
        bool p_EngineThrustOn = false;
        EngineThread EngineThread = new EngineThread();
        double p_SpatialScale = 1;
        double p_TimeScale = 1;
        double p_FramesPerSec = 0;
        Int32 p_OsdLevel = 0;
        Boolean p_Frozen = false;
        Vector p_ViewPoint = new Vector(0, 0);
        PhysicObject p_FocusedPhysicObject = null;
        double p_TimerInterval = 40;
        World p_World = new World();
        Font p_Font = new Font("Arial", 10, GraphicsUnit.Pixel);

        private System.Timers.Timer timer1;
        DateTime p_LastGCDate = DateTime.MinValue;

        Int32 maxSecondsEngineFireParticlesLivetime = 10;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public PhysicsView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timer1 = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.SynchronizingObject = this;
            // 
            // PhysicsView
            // 
            this.Name = "PhysicsView";
            this.Size = new System.Drawing.Size(120, 112);
            this.Resize += new System.EventHandler(this.PhysicsView_Resize);
            this.Load += new System.EventHandler(this.PhysicsView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PhysicsView_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PhysicsView_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PhysicsView_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PhysicsView_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();

        }
        #endregion

        private void PaintScala(Graphics g)
        {
            int x0 = this.Size.Width / 12;
            int y0 = this.Size.Height * 10 / 12;
            int x1 = this.Size.Width * 11 / 12;
            g.DrawLine(Pens.Gray, x0, y0, x1, y0);
            for (int i = 0; i < 11; i++)
            {
                int x = (int)(x0 + i * this.Size.Width / 12);
                g.DrawLine(Pens.Gray, x, y0 - 5, x, y0);
                g.DrawString((i * this.Size.Width / 12 / this.p_SpatialScale).ToString("F"), this.p_Font, Brushes.Gray, x, y0 + 5);
            }
        }

        private void PaintOnscreenText(Graphics g)
        {
            g.DrawString(this.GetOsdText(), this.p_Font, System.Drawing.Brushes.Black, 10, 10);
            g.DrawString(this.GetCommandHelText(), this.p_Font, System.Drawing.Brushes.Black, this.Size.Width - 150, 10);
            this.PaintScala(g);
        }

        private void PaintWorld(Graphics g)
        {
            lock (this.p_World.PhysicObjects)
            {
                foreach (PhysicObject physicObject in this.p_World.PhysicObjects)
                {
                    Vector viewPosition = physicObject.Position - this.p_ViewPoint;

                    viewPosition *= this.p_SpatialScale;

                    physicObject.Shape.Paint(g, viewPosition.X, viewPosition.Y, this.p_SpatialScale);

                    if (this.p_OsdLevel == 1)
                    {
                        if (physicObject.Name != String.Empty)
                        {
                            g.DrawString(this.PhysicObjectDescription(physicObject), this.p_Font, System.Drawing.Brushes.Black, (float)viewPosition.X - 10, (float)viewPosition.Y + 10);
                        }
                    }
                }
            }
        }

        private void PaintBackground(Graphics g)
        {
            g.FillRectangle(System.Drawing.Brushes.LightBlue, g.ClipBounds);
        }

        private void PhysicsView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                e.Graphics.FillRectangle(Brushes.Gray, 0, 0, this.Size.Width, this.Size.Height);
                return;
            }

            DateTime begin = DateTime.Now;

            if (e.ClipRectangle.Width == 0 || e.ClipRectangle.Height == 0) return;

            Bitmap bufl = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            using (Graphics g = Graphics.FromImage(bufl))
            {
                if (this.p_FocusedPhysicObject != null)
                {
                    this.p_ViewPoint = this.p_FocusedPhysicObject.Position
                        - new Vector(this.Size.Width / 2, this.Size.Height / 2) / this.p_SpatialScale;
                }

                this.PaintBackground(g);
                this.PaintOnscreenText(g);
                this.PaintWorld(g);

                e.Graphics.DrawImageUnscaled(bufl, 0, 0);
                g.Dispose();
            }

            DateTime end = DateTime.Now;
            TimeSpan execSpan = (end - begin);
            this.p_FramesPerSec = 1000 / execSpan.TotalMilliseconds;
        }

        private String FocusedDescription()
        {
            PhysicObject focused = this.p_FocusedPhysicObject;
            if (focused == null) return String.Empty;
            return String.Format("{0} X: {1:F}, Y: {2:F}, {3:F} m/s, {4:F} kg", focused.Name, focused.Position.X, focused.Position.Y, focused.Speed, focused.Mass);
        }

        private String GetCommandHelText()
        {
            return "B Brake\r\nK Fire Engine\r\nJ Turn Left\r\nLTurn Right\r\nSpace Fire Wapon\r\n"
                + "O Toggle OSD\r\nN Focus Next\r\nR Reset\r\nF Freeze\r\n"
                + "1 Realtime\r\nH 1h/s\r\nD 1d/s\r\nM 1Mth/s\r\nY 1a/s\r\n"
                + "W Increase Time Scale\r\nQ Decrease Time Scale\r\n"
                + "S Increase Spatial Scale\r\nA Decrease Spatial Scale";
        }

        private String GetOsdText()
        {
            String timeScale
                = (this.p_TimeScale >= CosmicConstants.Year)
                ? String.Format("{0:F} years", this.p_TimeScale / CosmicConstants.Year)
                : (this.p_TimeScale >= CosmicConstants.Month)
                ? String.Format("{0:F} months", this.p_TimeScale / CosmicConstants.Month)
                : (this.p_TimeScale >= CosmicConstants.Day)
                ? String.Format("{0:F} days", this.p_TimeScale / CosmicConstants.Day)
                : (this.p_TimeScale >= CosmicConstants.Hour)
                ? String.Format("{0:F} hours", this.p_TimeScale / CosmicConstants.Hour)
                : (this.p_TimeScale >= CosmicConstants.Minute)
                ? String.Format("{0:F} minutes", this.p_TimeScale / CosmicConstants.Minute)
                : String.Format("{0:F} seconds", this.p_TimeScale)
                ;
            String focusedOsd = String.Empty;
            PhysicObject focused = this.p_FocusedPhysicObject;
            if (focused != null)
            {
                Vector forceSum = Vector.NullVector;
                foreach (Engine e in focused.Engines)
                {
                    forceSum += e.Force;
                }
                focusedOsd = String.Format("Focused: {0}\r\nEngines Sum: {1:F} dx, {2:F} dy, {3:F} Nm\r\nGravitational force: {4:F} dx, {5:F} dy, {6:F} Nm", this.FocusedDescription(), forceSum.X, forceSum.Y, forceSum.Magnitude, focused.GravForce.X, focused.GravForce.Y, focused.GravForce.Magnitude);

            }
            return String.Format("Spatial Scale {0:F} meter per pixel\r\nTime Scale {1} per second\r\nFrames/second {2:F}\r\n{3}", 1 / this.p_SpatialScale, timeScale, this.p_FramesPerSec, focusedOsd);
        }

        private String PhysicObjectDescription(PhysicObject m)
        {
            return String.Format("{0}\r\n{1:F} kg\r\n{2:F} m/s\r\nX: {3:F}\r\nY: {4:F}", m.Name, m.Mass, m.Speed, m.Position.X, m.Position.Y);
        }

        private void PhysicsView_Load(object sender, System.EventArgs e)
        {
            this.EngineThread.Run();
#if false
			this.p_World.InsertSolarSystem();
			this.p_FocusedPhysicObject = this.p_World.FindPhysicObject("Sun");
			this.p_SpatialScale = this.Size.Width / (CosmicConstants.AU * 2) * 2/3;
#else
            PhysicObject nina = new PhysicObject("Nina", Vector.NullVector, 0, new Vector(1, 0), 1E+13);
            nina.Shape = new CircleShape(2);
            this.p_World.PhysicObjects.Add(nina);

            PhysicObject ninasMoon = new PhysicObject("Ninas Moon", new Vector(-10, 0), 6, new Vector(0, -1), 1E+10);
            ninasMoon.Shape = new CircleShape(1);
            this.p_World.PhysicObjects.Add(ninasMoon);

            PhysicObject starship = new PhysicObject("Starship Ed", Vector.NullVector + Vector.UnitVectorX * -40, 0, new Vector(1, 0), 1E+4);
            this.p_World.PhysicObjects.Add(starship);
            starship.Shape = new StarshipShape();
            starship.Shape.Orientation = new Vector(1, -0.618);

            Engine engine = new Engine(Vector.NullVector, 0/*-0.9*/);
            engine.MaxStrength = 10E+4;
            this.EngineThread.Add(engine);
            starship.Engines.Add(engine);

            this.p_SpatialScale = 6;
            this.p_FocusedPhysicObject = starship;

#endif

            this.timer1.Interval = (int)this.p_TimerInterval;
            this.timer1.Enabled = true;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(TimerTick);
            this.timer1.Start();
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
        }

        private static Object timerLock = new Object();

        private void TimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!System.Threading.Monitor.TryEnter(timerLock))
            {
                return;
            }

            lock (timerLock)
            {
                if (this.p_Frozen)
                {
                    this.Invalidate(false);
                    return;
                }
                Physics physics = new Physics();

                lock (this.p_World.PhysicObjects)
                {
                    PhysicObject[] physObjs = null;
                    if (this.p_World.PhysicObjects.Count > this.CountObjectsCalculationTreshold)
                    {
                        // Take only the biggest objects into the calculation
                        // 1. Sort the objects descending by mass
                        this.p_World.PhysicObjects.Sort(new Comparison<PhysicObject>(
                            delegate(PhysicObject a, PhysicObject b) { return -a.Mass.CompareTo(b.Mass); }));

                        physObjs = new PhysicObject[this.CountObjectsCalculationTreshold];

                        Int32 count = 0;
                        foreach (PhysicObject o in this.p_World.PhysicObjects)
                        {
                            physObjs[count++] = o;
                            if (count == this.CountObjectsCalculationTreshold) break;
                        }
                    }
                    else
                    {
                        physObjs = (PhysicObject[])this.p_World.PhysicObjects.ToArray();
                    }
                    physics.Calculate(physObjs, this.p_TimerInterval * this.p_TimeScale);
                }

                if (DateTime.Now >= this.p_LastGCDate + TimeSpan.FromMilliseconds(125))
                {
                    this.p_World.GC();
                    this.p_LastGCDate = DateTime.Now;
                }

                if (this.p_EngineThrustOn)
                {
                    this.FireEngine();
                }

                this.Invalidate(false);
            }
        }

        private void PhysicsView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.p_Frozen = true;
                try
                {
                    PhysicObjectForm form = new PhysicObjectForm();
                    form.PhysicObject = new PhysicObject();
                    form.PhysicObject.Position.X = this.p_ViewPoint.X + e.X / this.p_SpatialScale;
                    form.PhysicObject.Position.Y = this.p_ViewPoint.Y + e.Y / this.p_SpatialScale;

                    //form.PhysicObject.Position /= this.p_SpatialScale;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        lock (this.p_World.PhysicObjects)
                        {
                            this.p_World.PhysicObjects.Add(form.PhysicObject);
                        }
                    }
                }
                finally
                {
                    this.p_Frozen = false;
                }
            }
            else
            {
                this.p_FocusedPhysicObject = null;

                this.p_ViewPoint =
                    this.p_ViewPoint
                    + new Vector(e.X, e.Y) / this.p_SpatialScale
                    - new Vector(this.Size.Width / 2, this.Size.Height / 2) / this.p_SpatialScale;
            }
        }

        private void PhysicsView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.J: this.p_ViewPoint -= new Vector(10, 0) / this.p_SpatialScale; return;
                    case Keys.L: this.p_ViewPoint += new Vector(10, 0) / this.p_SpatialScale; return;
                    case Keys.I: this.p_ViewPoint -= new Vector(0, 10) / this.p_SpatialScale; return;
                    case Keys.K: this.p_ViewPoint += new Vector(0, 10) / this.p_SpatialScale; return;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.S: this.p_SpatialScale *= 2; return;
                case Keys.A: this.p_SpatialScale /= 2; return;

                case Keys.W: this.p_TimeScale *= 2; return;
                case Keys.Q: this.p_TimeScale /= 2; return;

                case Keys.H: this.p_TimeScale = CosmicConstants.Hour; return;
                case Keys.D: this.p_TimeScale = CosmicConstants.Day; return;
                case Keys.M: this.p_TimeScale = CosmicConstants.Month; return;
                case Keys.Y: this.p_TimeScale = CosmicConstants.Year; return;

                case Keys.D1:
                    this.p_SpatialScale = 1;
                    this.p_TimeScale = 1;
                    return;


                case Keys.F: this.p_Frozen = !this.p_Frozen; return;
                case Keys.R:
                    lock (this.p_World.PhysicObjects)
                    {
                        this.p_World.PhysicObjects.Clear();
                    }
                    return;
                case Keys.N:
                    if ((e.Modifiers & Keys.Shift) == Keys.Shift)
                    {
                        this.p_FocusedPhysicObject = null;
                        return;
                    }
                    if (this.p_World.PhysicObjects.Count == 0) return;
                    if (this.p_FocusedPhysicObject == null)
                        this.p_FocusedPhysicObject = (PhysicObject)this.p_World.PhysicObjects[0];
                    else
                    {
                        lock (this.p_World.PhysicObjects)
                        {
                            Int32 i = this.p_World.PhysicObjects.IndexOf(this.p_FocusedPhysicObject) + 1;
                            i = i % this.p_World.PhysicObjects.Count;
                            this.p_FocusedPhysicObject = (PhysicObject)this.p_World.PhysicObjects[i];
                        }
                    }
                    return;
                case Keys.L:
                    if (this.p_FocusedPhysicObject != null)
                    {
                        lock (this.p_World.PhysicObjects)
                        {
                            this.p_FocusedPhysicObject.Shape.Orientation = this.p_FocusedPhysicObject.Shape.Orientation.Rotate(CosmicConstants.PI / 30);
                        }
                    }
                    return;
                case Keys.J:
                    if (this.p_FocusedPhysicObject != null)
                    {
                        lock (this.p_World.PhysicObjects)
                        {
                            this.p_FocusedPhysicObject.Shape.Orientation = this.p_FocusedPhysicObject.Shape.Orientation.Rotate(-CosmicConstants.PI / 30);
                        }
                    }
                    return;
                case Keys.B:
                    if (this.p_FocusedPhysicObject != null)
                    {
                        lock (this.p_World.PhysicObjects)
                        {
                            this.p_FocusedPhysicObject.Speed /= 2;
                        }
                    }
                    return;

                case Keys.K:
                    this.FireEngine();
                    return;

                case Keys.Space:
                    this.FireWeapon();
                    return;

                case Keys.O:
                    this.p_OsdLevel = (this.p_OsdLevel + 1) % 2;
                    return;
            }
        }

        private void FireWeapon()
        {
            if (this.p_FocusedPhysicObject != null)
            {
                lock (this.p_World.PhysicObjects)
                {
                    PhysicObject bullet = new PhysicObject(String.Empty,
                        this.p_FocusedPhysicObject.Position,
                        this.p_FocusedPhysicObject.Speed + 50,
                        this.p_FocusedPhysicObject.Shape.Orientation,
                        2);
                    bullet.Shape = new CircleShape(System.Drawing.Pens.Chocolate, 0.001);
                    bullet.LiveUntil = DateTime.Now + TimeSpan.FromSeconds(4);
                    lock (this.p_World.PhysicObjects)
                    {
                        this.p_World.PhysicObjects.Add(bullet);
                    }
                }
            }
        }

        private void FireEngine()
        {
            if (this.p_FocusedPhysicObject != null)
            {
                lock (this.p_World.PhysicObjects)
                {
                    if (this.p_FocusedPhysicObject.Engines.Count > 0)
                    {
                        p_EngineThrustOn = true;
                        Engine e = (Engine)this.p_FocusedPhysicObject.Engines[0];
                        double strength = e.Force.Magnitude + e.MaxStrength;
                        e.Force = this.p_FocusedPhysicObject.Shape.Orientation.UnitVector * strength;

                        Vector particleCenter = this.p_FocusedPhysicObject.Position - this.p_FocusedPhysicObject.Shape.Orientation.UnitVector * 3 / 2;
                        for (int i = 0; i < 6; i++)
                        {
                            PhysicObject particle = new PhysicObject(String.Empty,
                                particleCenter + new Vector(0, 0.1 * (p_Randomizer.Next() % 10)).Rotate(((double)p_Randomizer.Next() % 180) / CosmicConstants.PI),
                                this.p_FocusedPhysicObject.Speed * 0.7,
                                this.p_FocusedPhysicObject.Direction,
                                0.01);
                            particle.Shape = new CircleShape(p_Randomizer.Next() % 2 == 0 ? System.Drawing.Pens.DarkGoldenrod : System.Drawing.Pens.DarkRed, 0.001);

                            particle.LiveUntil = DateTime.Now 
                                    + TimeSpan.FromMilliseconds(maxSecondsEngineFireParticlesLivetime * 1000 * 4 / 5 
                                                                    + p_Randomizer.Next() % maxSecondsEngineFireParticlesLivetime * 1000 * 1 / 5);
                            lock (this.p_World.PhysicObjects)
                            {
                                this.p_World.PhysicObjects.Add(particle);
                            }
                        }
                    }
                }
            }
        }

        private void PhysicsView_Resize(object sender, System.EventArgs e)
        {
        }

        public void Shutdown()
        {
            this.EngineThread.Shutdown = true;
        }

        private void PhysicsView_KeyUp(object sender, System.Windows.Forms.KeyEventArgs ea)
        {
            if (p_EngineThrustOn)
            {
                if (this.p_FocusedPhysicObject != null)
                {
                    lock (this.p_World.PhysicObjects)
                    {
                        if (this.p_FocusedPhysicObject.Engines.Count > 0)
                        {
                            this.p_EngineThrustOn = false;
                            Engine e = (Engine)this.p_FocusedPhysicObject.Engines[0];
                            e.Force = Vector.NullVector;
                        }
                    }
                }
                return;
            }

        }
    }
}
