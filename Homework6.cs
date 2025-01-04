using System;
using Xunit;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using HomeWorkFour;
using HomeWorkFive;

namespace HomeWorkSix
{
    class MovingAdapter : IMoving
    {
        private IDictionary<string, object> _map;
        public MovingAdapter (IDictionary<string, object> map)
        {
            _map = map;
        }
        public Vector GetLocation()
        {
            return (Vector)_map["Location"];
        }
        public Vector GetVelocity()
        {
            Angle angle = (Angle)_map["Angle"];
            int velocity = (int)_map["Velocity"];
            return new Vector(
                velocity * Math.Cos(angle.ToDouble()),
                velocity * Math.Sin(angle.ToDouble())
            );
        }
        public void SetLocation(Vector newValue)
        {
            _map["Location"] = newValue;
        }

    }
}

