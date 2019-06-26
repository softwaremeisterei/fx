using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx.Bots
{
    class MovingBehaviour : BotBehaviour
    {
        private static Random _randomizer = new Random();
        private DateTime _nextActionTime = DateTime.Now;

        public Action<Bot> StartEngine { get; internal set; }

        public Action<Bot> StopEngine { get; internal set; }

        public MovingBehaviour(Bot bot)
            : base(bot)
        { }

        public override void Calc(double dtms)
        {
            if (_nextActionTime <= DateTime.Now)
            {
                if (Bot.Engines.Any())
                {
                    if (Bot.Engines.Any(e => e.Force.Magnitude > 0))
                    {
                        StopEngine(Bot);
                        _nextActionTime = DateTime.Now + TimeSpan.FromMilliseconds(_randomizer.Next(1000));
                    }
                    else
                    {
                        StartEngine(Bot);
                        _nextActionTime = DateTime.Now + TimeSpan.FromMilliseconds(_randomizer.Next(200));
                    }
                }
            }
        }

        public override bool IsDone()
        {
            return false;
        }
    }
}
