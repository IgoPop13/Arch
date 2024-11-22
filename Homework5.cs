// Реализация IoC контейнер

// Цель: Реализовать собственный IoC контейнер, устойчивый к изменению требований.

// В результате выполнения домашнего задания Вы получите IoC, который можно будет использовать в качестве фасада в своих проектах.

// Описание/Пошаговая инструкция выполнения домашнего задания:
// В игре Космичекий бой есть набор операций над игровыми объектами: движение по прямой, поворот, выстрел. При этом содержание этих команд может отличаться для разных игр, в зависимости от того, какие правила игры были выбраны пользователями. Например, пользователи могут ограничить запас ход каждого корабля некоторым количеством топлива, а другой игре запретить поворачиваться кораблям по часовой стрелке и т.д.
// IoC может помочь в этом случае, скрыв детали в стратегии разрешения зависимости.
// Например,
// IoC.Resolve("двигаться прямо", obj);
// Возвращает команду, которая чаще всего является макрокомандой и осуществляет один шаг движения по прямой.
// Реализовать IoC контейнер, который:
// Разрешает зависимости с помощью метода, со следующей сигнатурой:
// IoC.Resolve<T>(string key, params object[] args);
// 
// Регистрация зависимостей также происходит с помощью метода Resolve
// IoC.Resolve("IoC.Register", "aaa", (args) => new A()).Execute();

// Зависимости можно регистрировать в разных "скоупах"
// IoC.Resolve("Scopes.New", "scopeId").Execute();
// IoC.Resolve("Scopes.Current", "scopeId").Exceute();

// Указание: Если Ваш фреймворк допускает работу с многопоточным кодом, то для работы со скоупами используйте ThreadLocal контейнер.

// Критерии оценки:
// Интерфейс IoC устойчив к изменению требований. Оценка: 0 - 3 балла (0 - совсем не устойчив, 3 - преподаватель не смог построить ни одного контрпримера)
// IoC предоставляет ровно один метод для всех операций. 1 балл
// IoC предоставляет работу со скоупами для предотвращения сильной связности. 2 балла.
// Реализованы модульные тесты. 2 балла
// Реализованы многопоточные тесты. 2 балла

using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeWorkFive
{
    IoC.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand((int)args[0],(string)args[1])).Execute();  

    class NoLocationException : Exception {}
    class NoVelocityException : Exception {}
    class NoMovementException : Exception {}
    class CommandException : Exception {}

    class RunTest
    {
        void CheckFuelCommandTest()
        {
            CheckFuelCommand checkFuelCommand;
            Fuel fuel;

            // TEST: CheckFuelCommand 
            
            // correct
            fuel = new Fuel(10, 1);
            checkFuelCommand = new CheckFuelCommand (fuel);
            try
            {
                checkFuelCommand.Execute();
            }
            catch (CommandException ce)
            {
                throw (new Exception("CheckFuelCommand success test failed"));
            }
            // test passed

            // exception
            fuel = new Fuel(1, 2);
            checkFuelCommand = new CheckFuelCommand (fuel);
            try
            {
                checkFuelCommand.Execute();
            }
            catch (CommandException ce)
            {
                // test passed
            }
            // test failed
            throw (new Exception("CheckFuelCommand unsuccess test failed"));
        }

        void BurnFuelCommandTest()
        {
            BurnFuelCommand burnFuelCommand;
            Fuel fuel;

            // TEST: BurnFuelCommand 
            
            // correct
            fuel = new Fuel(10, 1);
            burnFuelCommand = new BurnFuelCommand (fuel);
            try
            {
                burnFuelCommand.Execute();
            }
            catch (CommandException ce)
            {
                throw (new Exception("BurnFuelCommand success test failed"));
            }
            if (fuel.GetFuel() != 9)
                throw (new Exception("BurnFuelCommand success test failed"));
            // test passed

            // exception
            fuel = new Fuel(1, 2);
            burnFuelCommand = new BurnFuelCommand (fuel);
            try
            {
                burnFuelCommand.Execute();
            }
            catch (CommandException ce)
            {
                // test passed
            }
            // test failed
            throw (new Exception("BurnFuelCommand unsuccess test failed"));
        }

        void RectililearMoveWithFuelConsumptionCommandTest()
        {
            RectililearMoveWithFuelConsumptionCommand rectililearMoveWithFuelConsumptionCommand;
            Moving moving;
            Fuel fuel;

            // TEST: RectililearMoveWithFuelConsumptionCommand 
            
            // correct
            moving = new Moving(new Vector(0, 0), new Vector(1, 2));
            fuel = new Fuel(10, 1);
            rectililearMoveWithFuelConsumptionCommand = new RectililearMoveWithFuelConsumptionCommand (moving, fuel);
            try
            {
                rectililearMoveWithFuelConsumptionCommand.Execute();
            }
            catch (CommandException ce)
            {
                throw (new Exception("RectililearMoveWithFuelConsumptionCommand success test failed"));
            }
            if ((fuel.GetFuel() != 9) || (moving.GetLocation().X != 1) || (moving.GetLocation().Y != 2))
            {
                throw (new Exception("RectililearMoveWithFuelConsumptionCommand success test failed"));
            }
            // test passed

            // exception
            moving = new Moving(new Vector(0, 0), new Vector(1, 2));
            fuel = new Fuel(1, 1);
            rectililearMoveWithFuelConsumptionCommand = new RectililearMoveWithFuelConsumptionCommand (moving, fuel);
            try
            {
                rectililearMoveWithFuelConsumptionCommand.Execute();
            }
            catch (CommandException ce)
            {
                // test passed
            }
            // test failed
            throw (new Exception("RectililearMoveWithFuelConsumptionCommand unsuccess test failed"));
        }

        void ExecTests()
        {
            CheckFuelCommandTest();
            BurnFuelCommandTest();
        }
    }

    class RectililearMoveWithFuelConsumptionCommand : ICommand
    {
        private MoveCommand moveCommand;
        private BurnFuelCommand burnFuelCommand;
        private CheckFuelCommand checkFuelCommand;
        private MacroCommand macroCommand;
        public RectililearMoveWithFuelConsumptionCommand(Moving moving, Fuel fuel)
        {
            moveCommand = new MoveCommand(moving);
            checkFuelCommand = new CheckFuelCommand(fuel);
            burnFuelCommand = new BurnFuelCommand(fuel);
            macroCommand = new MacroCommand(new List<ICommand> {checkFuelCommand, burnFuelCommand, moveCommand});
        }
        public void Execute()
        {
            macroCommand.Execute();
        }
    }

    class MacroCommand : ICommand
    {
        private List<ICommand> _commands;
        public MacroCommand(List<ICommand> commands)
        {
            _commands = commands;
        }
        public void Execute()
        {
            foreach(ICommand command in _commands)
            {
                command.Execute();
            }
        }
    }

    interface ICommand
    {
        public void Execute();
    }

    class Vector
    {
        int _x;
        int _y;
        public Vector(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public static Vector Plus (Vector a, Vector b)
        {
            return new Vector (a.X + b.X, a.Y + b.Y);
        }
        public int X
        {
            get
            {
                return _x;
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
        }
    }

    class Angle
    {
        int _angle;
        int _n;
        public Angle (int angle, int n)
        {
            _angle = angle;
            _n = n;
        }
        public Angle Plus (int aVelocity)
        {
            return new Angle ((_angle + aVelocity) % _n, _n);
        }
        public int GetAngle()
        {
            return _angle;
        }
    }

    class Fuel
    {
        int _fuel;
        int _consumption;
        public Fuel (int fuel, int consumption)
        {
            _fuel = fuel;
            _consumption = consumption;
        }
        public void Plus (int delta)
        {
            _fuel += delta;
        }
        public int GetFuel()
        {
            return _fuel;
        }
        public int GetConsumption()
        {
            return _consumption;
        }
    }

    interface IFuelling
    {
        public int GetFuel();
        public void Fill(int credit);
        public void Burn(int debit);
        public bool Check();
        public int GetConsumption();
    }

    class Fuelling : IFuelling
    {
        private Fuel _fuel;
        public Fuelling(Fuel fuel)
        {
            _fuel = fuel;
        }
        public int GetFuel()
        {
            return _fuel.GetFuel();
        }
        public int GetConsumption()
        {
            return _fuel.GetConsumption();
        }
        public void Fill(int credit)
        {
            _fuel.Plus(credit);
        }
        public void Burn(int debit)
        {
            _fuel.Plus(-1 * debit);
        }
        public bool Check()
        {
            return (_fuel.GetFuel() - _fuel.GetConsumption()) < 0;
        }
    }

    class FillFuelCommand : ICommand
    {
        private IFuelling _fuel;
        private int _credit;
        public FillFuelCommand(Fuel fuel, int credit)
        {
            _fuel = new Fuelling(fuel);
            _credit = credit;
        }
        public void Execute()
        {
            _fuel.Fill(_credit);
        }
    }

    class BurnFuelCommand : ICommand
    {
        private IFuelling _fuel;
        public BurnFuelCommand(Fuel fuel)
        {
            _fuel = new Fuelling(fuel);
        }
        public void Execute()
        {
            _fuel.Burn(_fuel.GetConsumption());
        }
    }

    class CheckFuelCommand : ICommand
    {
        private IFuelling _fuel;
        public CheckFuelCommand(Fuel fuel)
        {
            _fuel = new Fuelling(fuel);
        }
        public void Execute()
        {
            if(!_fuel.Check()) throw new CommandException();
        }
    }

    interface IRotating
    {
        public Angle GetAngle();
        public int GetAngularVelocity();
        public void SetAngle(Angle newValue);
    }

    class Rotating : IRotating
    {
        private Angle _angle;
        private int _angularVelocity;
        public Rotating(Angle angle, int angularVelocity)
        {
            _angle = angle;
            _angularVelocity = angularVelocity;
        }
        public Angle GetAngle()
        {
            return _angle;
        }
        public int GetAngularVelocity()
        {
            return _angularVelocity;
        }
        public void SetAngle(Angle angle)
        {
            _angle = angle;
        }
    }

    class RotateCommand : ICommand
    {
        IRotating _rotating;
        public RotateCommand(IRotating rotating)
        {
            _rotating = rotating;
        }
        public void  Execute()
        {
            _rotating.SetAngle(_rotating.GetAngle().Plus(_rotating.GetAngularVelocity()));
        }
    }

    interface IMoving
    {
        public Vector GetLocation();
        public Vector GetVelocity();
        public void SetLocation(Vector newValue);
    }

    class MoveCommand : ICommand
    {
        IMoving _moving;
        public MoveCommand(IMoving moving)
        {
            _moving = moving;
        }
        public void  Execute()
        {
            _moving.SetLocation(Vector.Plus(_moving.GetLocation(), _moving.GetVelocity()));
        }
    }

    class Moving : IMoving
    {
        private Vector _location;
        private Vector _velocity;
        public Moving(Vector location, Vector velocity)
        {
            _location = location;
            _velocity = velocity;
        }
        public Vector GetLocation() { return _location; }
        public Vector GetVelocity() { return _velocity; }
        public void SetLocation(Vector location) { _location = location; }
    }
}
