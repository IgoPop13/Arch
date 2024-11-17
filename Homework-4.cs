// Макрокоманды

// Цель: Научиться обрабатывать ситуации с точки зрения SOLID, когда требуется уточнить существующее поведение без модификации существующего кода.

// Предположим, что у нас уже написаны команды MoveCommand и RotateCommand. Теперь возникло новое требование: пользователи в игре могут устанавливать правило - во время движение расходуется топливо, двигаться можно только при наличии топлива.

// Реализовать новую возможность можно введя две новые команды.
// CheckFuelCommand и BurnFuelCommand.
// CheckFuelCommand проверяет, что топлива достаточно, если нет, то выбрасывает исключение CommandException.
// BurnFuelCommand уменьшает количество топлива на скорость расхода топлива.

// После этого мы можем три команды выстроить в цепочку.
// CheckFuelCommand MoveCommand BurnFuelCommand

// Чтобы это было прозрачно для пользователя реализуем Макрокоманду - специальную разновидность команды, которая в конструкторе принимает массив команда а методе execute их все последовательно выполняет.

// При повороте движущегося объекта меняется вектор мгновенной скорости. Напишите команду, которая модифицирует вектор мгновенной скорости, в случае поворота.
// Постройте цепочку команд для поворота.

// Описание/Пошаговая инструкция выполнения домашнего задания:
// 1. Реализовать класс CheckFuelComamnd и тесты к нему.
// 2. Реализовать класс BurnFuelCommand и тесты к нему.
// 3. Реализовать простейшую макрокоманду и тесты к ней. Здесь простейшая - это значит, что при выбросе исключения вся последовательность команд приостанавливает свое выполнение, а макрокоманда выбрасывает CommandException.
// 4. Реализовать команду движения по прямой с расходом топлива, используя команды с предыдущих шагов.
// 5. Реализовать команду для модификации вектора мгновенной скорости при повороте. Необходимо учесть, что не каждый разворачивающийся объект движется.
// 6. Реализовать команду поворота, которая еще и меняет вектор мгновенной скорости, если есть.

// Критерии оценки:
// OK Домашнее задание сдано - 1 балл.
// OK (1) Реализована команда CheckFuelCommand - 1 балл
// OK (1) Написаны тесты к CheckFuelComamnd - 1 балл
// OK (2) Реализована команда BurnFuelCommand - 1 балл
// OK (2) Написаны тесты к BurnFuelComamnd - 1 балл
// OK (3) Реализована макрокоманда движения по прямой с расходом топлива и тесты к ней - 1 балл

// #########################################################################################
//                          ОБРАТИТЕ ВНИМАНИЕ НА КОММЕНТАРИЙ НИЖЕ
// #########################################################################################

// OK (4) Написаны тесты к MacroComamnd - 1 балл - COMMENT: поскольку предыдущая команда использует механизм универсальной макрокоманды

// (5) Реализована команда ChangeVelocityCommand - 1 балл
// (5) Написаны тесты к ChangeVelocityComamnd - 1 балл
// (6) Реализована команда поворота, которая еще и меняет вектор мгновенной скорости - 1 балл
// Итого: 10 баллов
// Задание принято, если набрано не менее 7 баллов.


using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeWorkFour
{

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

//    interface UObject
//    {
//        object this[string key]
//        {
//            get;
//            set;
//        }
//    }

//    class MovingAdapter : IMoving
//    {
//        UObject _obj;
//        public MovingAdapter(UObject obj)
//        {
//            _obj = obj;
//        }

//       public Vector GetLocation()
//       {
//            return (Vector) _obj["Location"];
//        }
        
//        public Vector GetVelocity()
//        {
//            Angle angle = (Angle) _obj["Angle"];
//            int velocity = (int) _obj["Velocity"];
//            return new Vector(velocity * Math.Cos(angle.ToDouble()), velocity * Math.Sin(angle.ToDouble()));
//        }
        
//        public void SetLocation(Vector newValue)
//        {
//            _obj["Location"] = newValue;
//        }
//    }
}
