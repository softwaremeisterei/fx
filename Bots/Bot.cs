using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bot : MassObject
{
    public List<BotBehaviour> Behaviours { get; set; }

    public void Calc(double dtms)
    {
        foreach (var botBehaviour in Behaviours)
        {
            botBehaviour.Calc(dtms);
        }
    }

    public Bot()
    {
        Shape = new BotShape();
        Behaviours = new List<BotBehaviour>();
    }
}
