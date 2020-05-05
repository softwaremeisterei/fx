using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx.Bots
{
    class FiringBehaviour : BotBehaviour
    {
        private static Random _randomizer = new Random();
        private DateTime _nextActionTime = DateTime.Now;

        public Action<Bot> FireWeapon { get; internal set; }

        public FiringBehaviour(Bot bot)
            : base(bot)
        { }

        public override void Calc(double dtms)
        {
            if (_nextActionTime <= DateTime.Now)
            {
                FireWeapon(Bot);
                _nextActionTime = DateTime.Now + TimeSpan.FromMilliseconds(_randomizer.Next(500));
            }
        }

        public override bool IsDone()
        {
            return false;
        }
    }
}
