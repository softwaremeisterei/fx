using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx.Bots
{
    class RotatingBehaviour : BotBehaviour
    {
        private static Random _randomizer = new Random();
        private DateTime _nextActionTime = DateTime.Now;
        private double _targetAngle;
        private double _rotationDirection = 1;

        public RotatingBehaviour(Bot bot)
            : base(bot)
        { _targetAngle = bot.Shape.Orientation.Angle; }

        public override void Calc(double dtms)
        {
            if (Math.Abs(_targetAngle - Bot.Shape.Orientation.Angle) < 0.1)
            {
                // target angle reached 
                _nextActionTime = DateTime.Now + TimeSpan.FromSeconds(_randomizer.Next(10));
            }
            else
            {
                Bot.Shape.Orientation = Bot.Shape.Orientation.Rotate(0.05 * _rotationDirection);
            }

            if (_nextActionTime <= DateTime.Now)
            {
                _targetAngle = _randomizer.NextDouble() % 2 * NaturalConstants.PI;
                _rotationDirection = _randomizer.Next() % 2 == 0 ? 1 : -1;
                _nextActionTime = DateTime.MaxValue;
            }
        }

        public override bool IsDone()
        {
            return false;
        }
    }
}
