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
            // return (Vector)_map["Location"];
            return IoC.Resolve<Vector>("IMoving.Location.Get", _map);
        }
        public Vector GetVelocity()
        {
            /*
            Angle angle = (Angle)_map["Angle"];
            int velocity = (int)_map["Velocity"];
            return new Vector(
                velocity * Math.Cos(angle.ToDouble()),
                velocity * Math.Sin(angle.ToDouble())
            );
            */
            return IoC.Resolve<Vector>("IMoving.Velocity.Get", _map);
        }
        public void SetLocation(Vector newValue)
        {
            // _map["Location"] = newValue;
            IoC.Resolve<ICommand>("IMoving.Location.Set", _map, newValue).Execute();
        }

    }

    // вынести в отдельный плагин, в котором прописать доступные объекты
    public class RegisterGameDependenciesCommand : ICommand
    {
        public RegisterGameDependenciesCommand()
        {
        }
        public void Execute()
        {
            (new InitCommand()).Execute();

            IoC.Resolve<ICommand>("IoC.Register", "IMoving.Location.Get", (object[] args) => new Vector((int)args[0], (int)args[1])).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "IMoving.Location.Set", (object[] args) => new Moving((Vector)args[0], (Vector)args[1])).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "IMoving.Velocity.Get", (object[] args) => new MoveCommand((Moving)args[0])).Execute();
            // зарегистрировать команды, брать параметры из Scope
        }
    }

    public class Game : ICommand
    {
        public Game()
        {
        }
        public void Execute()
        {
            new RegisterGameDependenciesCommand().Execute();
        }
    }

}

