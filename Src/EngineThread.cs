using System;
using System.Collections;
using System.Threading;

/// <summary>
/// Summary description for EngineThread.
/// </summary>
public class EngineThread
{
    public bool Shutdown = false;
    private ArrayList Engines = new ArrayList();

    private double ElapsedMs = 40;

    public void Add(Engine e)
    {
        lock (this.Engines)
        {
            this.Engines.Add(e);
        }
    }

    public void Run()
    {
        new Thread(new ThreadStart(WorkingThread)).Start();
    }

    public void Stop()
    {
        this.Shutdown = true;
        Thread.Sleep((int)this.ElapsedMs * 2);
    }

    void WorkingThread()
    {
        while (!this.Shutdown)
        {
            lock (this.Engines)
            {
                foreach (Engine e in this.Engines)
                {
                    if (e.Force.Magnitude != 0)
                    {
                        if ((DateTime.Now - e.LastUpdate).TotalMilliseconds >= 1000)
                        {
                            e.Force *= 1 + e.VariationPerSecond;
                            e.LastUpdate = DateTime.Now;
                        }
                        if (e.Force.Magnitude < 1E-3)
                        {
                            e.Force = Vector.NullVector;
                        }
                    }
                }
            }
            Thread.Sleep((int)this.ElapsedMs);
        }
    }
}
