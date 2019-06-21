using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

/// <summary>
/// Summary description for PhysicsView.
/// </summary>
public class PhysicsView : UserControl
{
    World _world = new World();
    MassObject _focusedObject = null;
    EngineThread _engineThread = new EngineThread();
    double _spatialScale = 1;
    double _timeScale = 1;
    Vector _viewPoint = new Vector(0, 0);
    double _framesPerSec = 0;
    int _osdLevel = 0;
    bool _isFrozen = false;

    readonly Random _randomizer = new Random();
    readonly Font _font = new Font("Arial", 10, GraphicsUnit.Pixel);

    private System.Timers.Timer timer1;

    const double _timerInterval = 40;
    const int maxSecondsEngineFireParticlesLivetime = 10;
    const int CountObjectsCalcTreshold = 200;

    private System.ComponentModel.Container components = null;

    public PhysicsView()
    {
        InitializeComponent();
    }

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
        this.SuspendLayout();
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
        this.Load += new System.EventHandler(this.PhysicsView_Load);
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.PhysicsView_Paint);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PhysicsView_MouseDown);
        ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
        this.ResumeLayout(false);

    }
    #endregion

    private void PaintScala(Graphics g)
    {
        int x0 = Size.Width / 12;
        int y0 = Size.Height * 10 / 12;
        int x1 = Size.Width * 11 / 12;
        g.DrawLine(Pens.Gray, x0, y0, x1, y0);

        for (int i = 0; i < 11; i++)
        {
            int x = (int)(x0 + i * Size.Width / 12);
            g.DrawLine(Pens.Gray, x, y0 - 5, x, y0);
            g.DrawString((i * Size.Width / 12 / _spatialScale).ToString("F"), _font, Brushes.Gray, x, y0 + 5);
        }
    }

    private void PaintOnscreenText(Graphics g)
    {
        g.DrawString(GetOsdText(), _font, System.Drawing.Brushes.Black, 10, 10);
        g.DrawString(GetCommandHelpText(), _font, System.Drawing.Brushes.Black, Size.Width - 150, 10);
        PaintScala(g);
    }

    private void PaintWorld(Graphics g)
    {
        lock (_world.Objects)
        {
            foreach (var obj in _world.Objects)
            {
                var viewPosition = obj.Position - _viewPoint;
                viewPosition *= _spatialScale;

                obj.Shape.Paint(g, viewPosition.X, viewPosition.Y, _spatialScale);

                if (_osdLevel == 1)
                {
                    if (obj.Name != String.Empty)
                    {
                        g.DrawString(Describe(obj), _font, System.Drawing.Brushes.Black, (float)viewPosition.X - 10, (float)viewPosition.Y + 10);
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
        if (DesignMode)
        {
            e.Graphics.FillRectangle(Brushes.Gray, 0, 0, Size.Width, Size.Height);
            return;
        }

        var begin = DateTime.Now;

        if (e.ClipRectangle.Width == 0 || e.ClipRectangle.Height == 0) return;

        Bitmap bufl = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
        using (Graphics g = Graphics.FromImage(bufl))
        {
            if (_focusedObject != null)
            {
                _viewPoint = _focusedObject.Position
                    - new Vector(Size.Width / 2, Size.Height / 2) / _spatialScale;
            }

            PaintBackground(g);
            PaintOnscreenText(g);
            PaintWorld(g);

            e.Graphics.DrawImageUnscaled(bufl, 0, 0);
            g.Dispose();
        }

        var end = DateTime.Now;
        _framesPerSec = 1000 / (end - begin).TotalMilliseconds;
    }

    private string FocusedDescription()
    {
        var focused = _focusedObject;
        if (focused == null) return String.Empty;
        return String.Format("{0} X: {1:F}, Y: {2:F}, {3:F} m/s, {4:F} kg", focused.Name, focused.Position.X, focused.Position.Y, focused.Speed, focused.Mass);
    }

    private string GetCommandHelpText()
    {
        return "B Brake\r\nK Fire Engine\r\nJ Turn Left\r\nLTurn Right\r\nSpace Fire Wapon\r\n"
            + "O Toggle OSD\r\nN Focus Next\r\nR Reset\r\nF Freeze\r\n"
            + "1 Realtime\r\nH 1h/s\r\nD 1d/s\r\nM 1Mth/s\r\nY 1a/s\r\n"
            + "W Increase Time Scale\r\nQ Decrease Time Scale\r\n"
            + "S Increase Spatial Scale\r\nA Decrease Spatial Scale";
    }

    private string GetOsdText()
    {
        var timeScale
            = (_timeScale >= NaturalConstants.Year) ? string.Format("{0:F} years", _timeScale / NaturalConstants.Year)
            : (_timeScale >= NaturalConstants.Month) ? string.Format("{0:F} months", _timeScale / NaturalConstants.Month)
            : (_timeScale >= NaturalConstants.Day) ? string.Format("{0:F} days", _timeScale / NaturalConstants.Day)
            : (_timeScale >= NaturalConstants.Hour) ? string.Format("{0:F} hours", _timeScale / NaturalConstants.Hour) : (_timeScale >= NaturalConstants.Minute)
                ? string.Format("{0:F} minutes", _timeScale / NaturalConstants.Minute) : string.Format("{0:F} seconds", _timeScale);
        var focusedOsd = string.Empty;
        var focused = _focusedObject;

        if (focused != null)
        {
            var force = Vector.NullVector;

            foreach (Engine e in focused.Engines)
            {
                force += e.Force;
            }

            focusedOsd = string.Format("Focused: {0}\r\nEngines Sum: {1:F} dx, {2:F} dy, {3:F} Nm\r\nGravitational force: {4:F} dx, {5:F} dy, {6:F} Nm", FocusedDescription(), force.X, force.Y, force.Magnitude, focused.GravForce.X, focused.GravForce.Y, focused.GravForce.Magnitude);

        }

        return string.Format("Spatial Scale {0:F} meter per pixel\r\nTime Scale {1} per second\r\nFrames/second {2:F}\r\n{3}", 1 / _spatialScale, timeScale, _framesPerSec, focusedOsd);
    }

    private string Describe(MassObject m)
    {
        return string.Format("{0}\r\n{1:F} kg\r\n{2:F} m/s\r\nX: {3:F}\r\nY: {4:F}", m.Name, m.Mass, m.Speed, m.Position.X, m.Position.Y);
    }

    private void PhysicsView_Load(object sender, System.EventArgs e)
    {
        _engineThread.Run();
#if false
			_world.InsertSolarSystem();
			_focusedObject = _world.FindPhysicObject("Sun");
			_spatialScale = Size.Width / (CosmicConstants.AU * 2) * 2/3;
#else
        var nina = new MassObject("Nina", Vector.NullVector, 0, new Vector(1, 0), 1E+13);
        nina.Shape = new CircleShape(2);
        _world.Objects.Add(nina);

        var ninasMoon = new MassObject("Ninas Moon", new Vector(-10, 0), 6, new Vector(0, -1), 1E+10);
        ninasMoon.Shape = new CircleShape(1);
        _world.Objects.Add(ninasMoon);

        var starship = new MassObject("Starship Ed", Vector.NullVector + Vector.UnitVectorX * -40, 0, new Vector(1, 0), 1E+4);
        _world.Objects.Add(starship);
        starship.Shape = new StarshipShape();
        starship.Shape.Orientation = new Vector(1, -0.618);

        var engine = new Engine(Vector.NullVector, 0/*-0.9*/);
        engine.MaxStrength = 10E+4;
        _engineThread.Add(engine);
        starship.Engines.Add(engine);

        _spatialScale = 6;
        _focusedObject = starship;

#endif

        timer1.Interval = (int)_timerInterval;
        timer1.Enabled = true;
        timer1.Elapsed += new System.Timers.ElapsedEventHandler(TimerTick);
        timer1.Start();

        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
    }

    private static Object Mux = new Object();

    private void TimerTick(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (!Monitor.TryEnter(Mux)) return;

        lock (Mux)
        {
            if (_isFrozen)
            {
                Invalidate(false);
                return;
            }

            HandleKeyboard();

            var physics = new Physics();

            lock (_world.Objects)
            {
                MassObject[] physObjs = null;

                if (_world.Objects.Count > CountObjectsCalcTreshold)
                {
                    // Take only the biggest objects into the calculation
                    // 1. Sort the objects descending by mass
                    _world.Objects.Sort(new Comparison<MassObject>((a, b) => -a.Mass.CompareTo(b.Mass)));

                    physObjs = new MassObject[CountObjectsCalcTreshold];

                    var count = 0;
                    foreach (MassObject o in _world.Objects)
                    {
                        physObjs[count++] = o;
                        if (count == CountObjectsCalcTreshold) break;
                    }
                }
                else
                {
                    physObjs = _world.Objects.ToArray();
                }

                physics.Calculate(physObjs, _timerInterval * _timeScale);
            }

            _world.CollectGarbage();

            Invalidate(false);
        }
    }

    private void HandleKeyboard()
    {
        if (Control.ModifierKeys == Keys.Shift)
        {
            if (Keyboard.IsKeyDown(Key.J)) { _viewPoint -= new Vector(10, 0) / _spatialScale; }
            if (Keyboard.IsKeyDown(Key.L)) { _viewPoint += new Vector(10, 0) / _spatialScale; }
            if (Keyboard.IsKeyDown(Key.I)) { _viewPoint -= new Vector(0, 10) / _spatialScale; }
            if (Keyboard.IsKeyDown(Key.K)) { _viewPoint += new Vector(0, 10) / _spatialScale; }

            return;
        }

        if (Keyboard.IsKeyDown(Key.S)) { _spatialScale *= 2; }
        if (Keyboard.IsKeyDown(Key.A)) { _spatialScale /= 2; }

        if (Keyboard.IsKeyDown(Key.W)) { _timeScale *= 2; }
        if (Keyboard.IsKeyDown(Key.Q)) { _timeScale /= 2; }

        if (Keyboard.IsKeyDown(Key.H)) { _timeScale = NaturalConstants.Hour; }
        if (Keyboard.IsKeyDown(Key.D)) { _timeScale = NaturalConstants.Day; }
        if (Keyboard.IsKeyDown(Key.M)) { _timeScale = NaturalConstants.Month; }
        if (Keyboard.IsKeyDown(Key.Y)) { _timeScale = NaturalConstants.Year; }

        if (Keyboard.IsKeyDown(Key.D1)) { _spatialScale = 1; _timeScale = 1; }


        if (Keyboard.IsKeyDown(Key.F)) { _isFrozen = !_isFrozen; }
        if (Keyboard.IsKeyDown(Key.R)) { lock (_world.Objects) { _world.Objects.Clear(); } }

        if (Keyboard.IsKeyDown(Key.N))
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                _focusedObject = null;
            }
            else
            {
                if (_world.Objects.Count > 0)
                {
                    if (_focusedObject == null)
                    {
                        _focusedObject = (MassObject)_world.Objects[0];
                    }
                    else
                    {
                        lock (_world.Objects)
                        {
                            Int32 i = _world.Objects.IndexOf(_focusedObject) + 1;
                            i = i % _world.Objects.Count;
                            _focusedObject = (MassObject)_world.Objects[i];
                        }
                    }
                }
            }
        }

        if (_focusedObject != null)
        {
            if (Keyboard.IsKeyDown(Key.L))
            {
                lock (_world.Objects)
                {
                    _focusedObject.Shape.Orientation = _focusedObject.Shape.Orientation.Rotate(NaturalConstants.PI / 20);
                }
            }

            if (Keyboard.IsKeyDown(Key.J))
            {
                lock (_world.Objects)
                {
                    _focusedObject.Shape.Orientation =
                        _focusedObject.Shape.Orientation.Rotate(-NaturalConstants.PI / 20);
                }
            }

            if (Keyboard.IsKeyDown(Key.B))
            {
                lock (_world.Objects)
                {
                    _focusedObject.Speed /= 2;
                }
            }
        }

        if (Keyboard.IsKeyDown(Key.K)) { StartEngine(); } else { StopEngine(); }

        if (Keyboard.IsKeyDown(Key.Space)) { FireWeapon(); }
        if (Keyboard.IsKeyDown(Key.O)) { _osdLevel = (_osdLevel + 1) % 2; }
    }

    private void PhysicsView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isFrozen = true;

            try
            {
                var form = new MassObjectDetails
                {
                    MassObject = new MassObject
                    {
                        Position = new Vector(_viewPoint.X + e.X / _spatialScale, _viewPoint.Y + e.Y / _spatialScale)
                    }
                };

                if (form.ShowDialog() == DialogResult.OK)
                {
                    lock (_world.Objects)
                    {
                        _world.Objects.Add(form.MassObject);
                    }
                }
            }
            finally
            {
                _isFrozen = false;
            }
        }
        else
        {
            _focusedObject = null;
            _viewPoint = _viewPoint + new Vector(e.X, e.Y) / _spatialScale - new Vector(Size.Width / 2, Size.Height / 2) / _spatialScale;
        }
    }

    private void FireWeapon()
    {
        if (_focusedObject != null)
        {
            lock (_world.Objects)
            {
                var bullet = new MassObject(String.Empty,
                    _focusedObject.Position,
                    _focusedObject.Speed + 50,
                    _focusedObject.Shape.Orientation,
                    2);
                bullet.Shape = new CircleShape(System.Drawing.Pens.Chocolate, 0.001);
                bullet.LiveUntil = DateTime.Now + TimeSpan.FromSeconds(4);
                lock (_world.Objects)
                {
                    _world.Objects.Add(bullet);
                }
            }
        }
    }

    private void StartEngine()
    {
        if (_focusedObject != null)
        {
            lock (_world.Objects)
            {
                if (_focusedObject.Engines.Count > 0)
                {
                    var engine = (Engine)_focusedObject.Engines[0];
                    double strength = engine.Force.Magnitude + engine.MaxStrength;
                    engine.Force = _focusedObject.Shape.Orientation.UnitVector * strength;

                    var particleCenter = _focusedObject.Position - _focusedObject.Shape.Orientation.UnitVector * 3 / 2;

                    for (int i = 0; i < 6; i++)
                    {
                        var particle = new MassObject(String.Empty,
                            particleCenter + new Vector(0, 0.1 * (_randomizer.Next() % 10)).Rotate(((double)_randomizer.Next() % 180) / NaturalConstants.PI),
                            _focusedObject.Speed * 0.7,
                            _focusedObject.Direction,
                            0.01);
                        particle.Shape = new CircleShape(_randomizer.Next() % 2 == 0 ? System.Drawing.Pens.DarkGoldenrod : System.Drawing.Pens.DarkRed, 0.001);

                        particle.LiveUntil = DateTime.Now + TimeSpan.FromMilliseconds(maxSecondsEngineFireParticlesLivetime * 1000 * 4 / 5
                                                          + _randomizer.Next() % maxSecondsEngineFireParticlesLivetime * 1000 * 1 / 5);
                        lock (_world.Objects)
                        {
                            _world.Objects.Add(particle);
                        }
                    }
                }
            }
        }
    }

    private void StopEngine()
    {
        if (_focusedObject != null)
        {
            lock (_world.Objects)
            {
                if (_focusedObject.Engines.Count > 0)
                {
                    var engine = (Engine)_focusedObject.Engines[0];
                    engine.Force = Vector.NullVector;
                }
            }
        }
    }

    public void Shutdown()
    {
        _engineThread.Shutdown = true;
    }

}
