using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using fx;
using fx.Bots;

/// <summary>
/// Summary description for PhysicsView.
/// </summary>
public class PhysicsView : UserControl
{
    World _world = new World();
    MassObject _focusedObject = null;
    Radar _radar;
    Scala _scala;

    EngineThread _engineThread = new EngineThread();

    double _spatialScale = 1;
    double _timeScale = 1;
    Vector _viewPoint = new Vector(0, 0);
    double _framesPerSec = 0;
    int _osdLevel = 0;
    bool _isFrozen = false;

    Font _font;
    private System.Timers.Timer physicsTimer;
    private System.Timers.Timer collisionsTimer;

    const double _physicsTimerInterval = 40;
    const double _collisionsTimerInterval = 40;

    const int maxSecondsEngineFireParticlesLivetime = 10;
    const int CountObjectsCalcThreshold = 300;

    private System.ComponentModel.Container components = null;
    private Random _randomizer = new Random();

    private DateTime _startTime = DateTime.Now;

    private double WeaponMass = 2;
    private double DebrisMass = 6;

    public PhysicsView()
    {
        InitializeComponent();

        _font = new Font("Arial", 9, GraphicsUnit.Pixel);
        _radar = new Radar { Position = new Vector(25, 100), Radius = 20, RealRadius = 100 };
        _scala = new Scala { Size = new System.Drawing.Size(500, 500), Font = _font };
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
        this.physicsTimer = new System.Timers.Timer();
        ((System.ComponentModel.ISupportInitialize)(this.physicsTimer)).BeginInit();
        this.collisionsTimer = new System.Timers.Timer();
        ((System.ComponentModel.ISupportInitialize)(this.collisionsTimer)).BeginInit();
        this.SuspendLayout();
        // 
        // timer1
        // 
        this.physicsTimer.Enabled = true;
        this.physicsTimer.SynchronizingObject = this;
        // 
        // timer1
        // 
        this.collisionsTimer.Enabled = true;
        this.collisionsTimer.SynchronizingObject = this;
        // 
        // PhysicsView
        // 
        this.Name = "PhysicsView";
        this.Size = new System.Drawing.Size(120, 112);
        this.Load += new System.EventHandler(this.PhysicsView_Load);
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.PhysicsView_Paint);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PhysicsView_MouseDown);
        this.Resize += new System.EventHandler(this.PhysicsView_Resize);
        ((System.ComponentModel.ISupportInitialize)(this.physicsTimer)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.collisionsTimer)).EndInit();
        this.ResumeLayout(false);

    }
    #endregion

    private void PaintOnscreenText(Graphics g)
    {
        g.DrawString(GetOsdText(), _font, Brushes.Black, 10, 10);
        if (DateTime.Now - _startTime < TimeSpan.FromSeconds(5))
        {
            g.DrawString(GetCommandHelpText(), _font, Brushes.Black, Size.Width - 100, 10);
        }
        _scala.Paint(g);
    }

    private void PaintWorld(Graphics g)
    {
        lock (_world.Objects)
        {
            if (_focusedObject != null)
            {
                _radar.Paint(g, _focusedObject.Position,
                    _world.Objects.Where(o => o.Name != null && o.Name.Length > 0 && o.Mass > DebrisMass).ToArray()
                        .Select(o => o.Position).ToArray());
            }

            foreach (var obj in _world.Objects)
            {
                var viewPosition = obj.Position - _viewPoint;
                viewPosition *= _spatialScale;

                obj.Shape.Paint(g, viewPosition.X, viewPosition.Y, _spatialScale);

                if (_osdLevel == 1)
                {
                    if (obj.Name != String.Empty)
                    {
                        g.DrawString(Describe(obj), _font, Brushes.Black, (float)viewPosition.X - 10, (float)viewPosition.Y + 10);
                    }
                }
            }
        }
    }

    private void PaintBackground(Graphics g)
    {
        g.FillRectangle(Brushes.LightBlue, g.ClipBounds);
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
        return "J Turn left\nL Turn right\nK Fire engine\nB Brake\nSpace Fire weapon\n"
            + "O Toggle OSD\nN Focus next\nR Reset\nF Freeze\n"
            + "S Zoom in\nA Zoom out";
    }

    private string GetOsdText()
    {
        var focused = _focusedObject;
        return
            $"FPS {_framesPerSec:F}\n" +
            $"Objects: {_world.Objects.Count}\n";
    }

    private string Describe(MassObject m)
    {
        return string.Format("{0}\n{1:F} kg\n{2:F} m/s\nX: {3:F}nY: {4:F}", m.Name, m.Mass, m.Speed, m.Position.X, m.Position.Y);
    }

    private void PhysicsView_Load(object sender, System.EventArgs e)
    {
        _spatialScale = 6;
        _scala.SpatialScale = _spatialScale;

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

        var starship = new MassObject("Starship Ed", Vector.NullVector + Vector.UnitVectorX * -20, 0, new Vector(1, 0), 1E+4);
        starship.Shape = new StarshipShape();
        starship.Shape.Orientation = new Vector(1, -0.618);
        starship.Direction = starship.Shape.Orientation;
        starship.Speed = 4;
        _world.Objects.Add(starship);

        _focusedObject = starship;

        var engine = new Engine(Vector.NullVector, 0/*-0.9*/);
        engine.MaxStrength = 10E+4;
        starship.Engines.Add(engine);
        _engineThread.Add(engine);

#endif

        physicsTimer.Interval = (int)_physicsTimerInterval;
        physicsTimer.Enabled = true;
        physicsTimer.Elapsed += new System.Timers.ElapsedEventHandler(PhysicsTimerTick);
        physicsTimer.Start();

        collisionsTimer.Interval = (int)_collisionsTimerInterval;
        collisionsTimer.Enabled = true;
        collisionsTimer.Elapsed += new System.Timers.ElapsedEventHandler(CollisionsTimerTick);
        collisionsTimer.Start();

        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
    }

    private static object Mux = new object();

    private void PhysicsTimerTick(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (!Monitor.TryEnter(Mux)) return;

        lock (Mux)
        {
            HandleKeyboard();

            if (_isFrozen == false)
            {
                var physics = new Physics();

                lock (_world.Objects)
                {
                    MassObject[] physObjs = null;

                    if (_world.Objects.Count > CountObjectsCalcThreshold)
                    {
                        // Take only the biggest objects into the calculation
                        // 1. Sort the objects descending by mass
                        _world.Objects.Sort(new Comparison<MassObject>((a, b) => -a.Mass.CompareTo(b.Mass)));

                        physObjs = new MassObject[CountObjectsCalcThreshold];

                        var count = 0;
                        foreach (MassObject o in _world.Objects)
                        {
                            physObjs[count++] = o;
                            if (count == CountObjectsCalcThreshold) break;
                        }
                    }
                    else
                    {
                        physObjs = _world.Objects.ToArray();
                    }

                    physics.Calculate(physObjs, _physicsTimerInterval * _timeScale);
                }
            }

            foreach (Bot bot in _world.Objects.Where(o => o is Bot).ToList())
            {
                bot.Calc(_physicsTimerInterval * _timeScale);
            }

            _world.CollectGarbage();
            Invalidate(false);
        }
    }

    private void CollisionsTimerTick(object sender, System.Timers.ElapsedEventArgs e)
    {
        var dict = new Dictionary<int, List<MassObject>>();

        lock (_world.Objects)
        {
            foreach (var o in _world.Objects.Where(o => o.Name != "Fuel"))
            {
                var pos = (int)o.Position.X * 10000 / 30  + (int)o.Position.Y * 100 / 30;
                List<MassObject> list;
                if (!dict.TryGetValue(pos, out list))
                {
                    list = new List<MassObject>();
                    dict.Add(pos, list);
                }
                list.Add(o);
            }

            foreach (var collisions in dict.Values.Where(l => l.Count > 1))
            {
                foreach (var massObject in collisions)
                {
                    if (massObject.Name == "Weapon" || massObject.Name == "Debris") continue;

                    if (massObject == _focusedObject) continue;

                    Explode(massObject);

                    _world.Objects.Remove(massObject);
                }
            }
        }
    }

    private void Explode(MassObject massObject)
    {
        massObject.LiveUntil = DateTime.Now;

        for (var i = 0; i < 20 + _randomizer.Next(20); i++)
        {
            var direction = massObject.Shape.Orientation.Rotate(_randomizer.NextDouble() % (2 * NaturalConstants.PI)).UnitVector;

            var debris = new MassObject("Debris",
                massObject.Position + direction * 3,
                5 + massObject.Speed * (1 + _randomizer.NextDouble()),
                direction,
                DebrisMass);
            debris.Shape = new CircleShape(Pens.IndianRed, 0.003);
            debris.LiveUntil = DateTime.Now + TimeSpan.FromSeconds(1 + _randomizer.Next(5));
            _world.Objects.Add(debris);
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

        if (Keyboard.IsKeyDown(Key.S)) { _spatialScale = Math.Min(1000, _spatialScale * 2); _scala.SpatialScale = _spatialScale; }
        if (Keyboard.IsKeyDown(Key.A)) { _spatialScale = Math.Max(0.001, _spatialScale / 2); _scala.SpatialScale = _spatialScale; }

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
                        _focusedObject = _world.Objects[0];
                    }
                    else
                    {
                        lock (_world.Objects)
                        {
                            var i = _world.Objects.IndexOf(_focusedObject) + 1;
                            i = i % _world.Objects.Count;
                            _focusedObject = _world.Objects[i];
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

        if (Keyboard.IsKeyDown(Key.Space)) { FireWeapon(_focusedObject); }
        if (Keyboard.IsKeyDown(Key.O)) { _osdLevel = (_osdLevel + 1) % 2; }

        if (Keyboard.IsKeyDown(Key.M)) { CreateBot(); }
    }

    private void CreateBot()
    {
        var focused = _focusedObject;
        if (focused != null)
        {
            var bot = new Bot { Name = "Bot", Shape = new BotShape() };
            bot.Mass = 2E+4;
            bot.Shape.Orientation = new Vector(_randomizer.NextDouble(), _randomizer.NextDouble());
            bot.Position = focused.Position + new Vector(_randomizer.Next(100) - 50, _randomizer.Next(100) - 50).UnitVector * (20 + _randomizer.Next(10));
            bot.Speed = _randomizer.Next(15);
            bot.LiveUntil = DateTime.Now + TimeSpan.FromSeconds(60 + _randomizer.Next(240));
            bot.Behaviours.Add(new FiringBehaviour(bot) { FireWeapon = FireWeapon });
            bot.Behaviours.Add(new RotatingBehaviour(bot));
            _world.Objects.Add(bot);
        }
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
                    MassObject = new MassObject { Position = new Vector(_viewPoint.X + e.X / _spatialScale, _viewPoint.Y + e.Y / _spatialScale) }
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

    private void FireWeapon(MassObject origin)
    {
        if (origin != null)
        {
            lock (_world.Objects)
            {
                var bullet = new MassObject("Weapon",
                    origin.Position + origin.Shape.Orientation.UnitVector * 2,
                    origin.Speed + 50,
                    origin.Shape.Orientation,
                    WeaponMass);
                bullet.Shape = new CircleShape(Pens.Chocolate, 0.001);
                bullet.LiveUntil = DateTime.Now + TimeSpan.FromSeconds(4);
                _world.Objects.Add(bullet);
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
                    var engine = _focusedObject.Engines[0];
                    var strength = engine.Force.Magnitude + engine.MaxStrength;
                    engine.Force = _focusedObject.Shape.Orientation.UnitVector * strength;

                    var particleCenter = _focusedObject.Position - _focusedObject.Shape.Orientation.UnitVector * 3 / 2;

                    for (int i = 0; i < 6; i++)
                    {
                        var particle = new MassObject("Fuel",
                            particleCenter + new Vector(0, 0.1 * (_randomizer.Next() % 10)).Rotate(((double)_randomizer.Next() % 180) / NaturalConstants.PI),
                            _focusedObject.Speed * 0.7,
                            _focusedObject.Direction,
                            0.01);
                        particle.Shape = new CircleShape(_randomizer.Next() % 2 == 0 ? Pens.DarkGoldenrod : Pens.DarkRed, 0.001);

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

    private void PhysicsView_Resize(object sender, EventArgs e)
    {
        _scala.Size = this.Size;
    }
}
