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
// Реализовать класс CheckFuelComamnd и тесты к нему.
// Реализовать класс BurnFuelCommand и тесты к нему.
// Реализовать простейшую макрокоманду и тесты к ней. Здесь простейшая - это значит, что при выбросе исключения вся последовательность команд приостанавливает свое выполнение, а макрокоманда выбрасывает CommandException.
// Реализовать команду движения по прямой с расходом топлива, используя команды с предыдущих шагов.
// Реализовать команду для модификации вектора мгновенной скорости при повороте. Необходимо учесть, что не каждый разворачивающийся объект движется.
// Реализовать команду поворота, которая еще и меняет вектор мгновенной скорости, если есть.

// Критерии оценки:
// Домашнее задание сдано - 1 балл.
// Реализована команда CheckFuelCommand - 1балл
// Написаны тесты к CheckFuelComamnd - 1 балл
// Реализована команда BurnFuelCommand - 1балл
// Написаны тесты к BurnFuelComamnd - 1 балл
// Реализована макрокоманда движения по прямой с расходом топлива и тесты к ней - 1 балл
// Написаны тесты к MacroComamnd - 1 балл
// Реализована команда ChangeVelocityCommand - 1балл
// Написаны тесты к ChangeVelocityComamnd - 1 балл
// Реализована команда поворота, которая еще и меняет вектор мгновенной скорости - 1балл
// Итого: 10 баллов
// Задание принято, если набрано не менее 7 баллов.


using System;

namespace HomeWorkFour
{

    interface ICommand
    {
        public void Execute();
    }

    class Command : ICommand
    {
        public Command()
        {
        }
        public void Execute()
        {
        }
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
    
    interface IRotating
    {
        public Angle GetAngle();
        public int GetAngularVelocity();
        public void SetAngle(Angle newValue);
    }

    class Rotate
    {
        IRotating _rotating;
        public Rotate(IRotating rotating)
        {
            _rotating = rotating;
        }
        public void  Execute()
        {
            _rotating.SetAngle(_rotating.GetAngle().Plus(_rotating.GetAngularVelocity()));
        }
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

    interface IMoving
    {
        public Vector GetLocation();
        public Vector GetVelocity();
        public void SetLocation(Vector newValue);
    }

    class Move
    {
        IMoving _moving;
        public Move(IMoving moving)
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

    class NoLocationException : Exception {}
    class NoVelocityException : Exception {}
    class NoMovementException : Exception {}

    class RunTest
    {
        void Exec()
        {
            Rotate rotate;
            Move move;
            // correct
            rotate = new Rotate (new Rotable (new Angle(0, 16), 3));
            move = new Move (new Movable(new Vector(12, 5), new Vector(-7, 3)));
            move.Execute();

            // exceptions
            move = new Move (new MovableNoLocation(new Vector(12, 5), new Vector(-7, 3)));
            move.Execute();
            move = new Move (new MovableNoVelocity(new Vector(12, 5), new Vector(-7, 3)));
            move.Execute();
            move = new Move (new MovableCantMove(new Vector(12, 5), new Vector(-7, 3)));
            move.Execute();
        }
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
