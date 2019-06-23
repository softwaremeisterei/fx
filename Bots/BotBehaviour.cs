using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BotBehaviour
{
    protected Bot Bot { get; }

    public BotBehaviour(Bot bot)
    {
        Bot = bot;
    }

    public abstract void Calc(double dtms);
    public abstract bool IsDone();
}
