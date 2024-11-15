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
    
    interface IRotable
    {
        public Angle GetAngle();
        public int GetAngularVelocity();
        public void SetAngle(Angle newValue);
    }

    class Rotate
    {
        IRotable _rotable;
        public Rotate(IRotable rotable)
        {
            _rotable = rotable;
        }
        public void  Execute()
        {
            _rotable.SetAngle(_rotable.GetAngle().Plus(_rotable.GetAngularVelocity()));
        }
    }

    class Rotable : IRotable
    {
        private Angle _angle;
        private int _angularVelocity;
        public Rotable(Angle angle, int angularVelocity)
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


    interface IMovable
    {
        public Vector GetLocation();
        public Vector GetVelocity();
        public void SetLocation(Vector newValue);
    }

    class Move
    {
        IMovable _movable;
        public Move(IMovable movable)
        {
            _movable = movable;
        }
        public void  Execute()
        {
            _movable.SetLocation(Vector.Plus(_movable.GetLocation(), _movable.GetVelocity()));
        }
    }

    class Movable : IMovable 
    {
        private Vector _location;
        private Vector _velocity;
        public Movable(Vector location, Vector velocity)
        {
            _location = location;
            _velocity = velocity;
        }
        public Vector GetLocation() { return _location; }
        public Vector GetVelocity() { return _velocity; }
        public void SetLocation(Vector location) { _location = location; }
    }

    class MovableNoLocation : IMovable // MOC
    {
        private Vector _location;
        private Vector _velocity;
        public MovableNoLocation(Vector location, Vector velocity)
        {
            _location = location;
            _velocity = velocity;
        }
        public Vector GetLocation() { throw new NoLocationException(); } // "Can't get location."
        public Vector GetVelocity() { return _velocity; }
        public void SetLocation(Vector location) { _location = location; }
    }

    class MovableNoVelocity : IMovable
    {
        private Vector _location;
        private Vector _velocity;
        public MovableNoVelocity(Vector location, Vector velocity)
        {
            _location = location;
            _velocity = velocity;
        }
        public Vector GetLocation() { return _location; }
        public Vector GetVelocity() { throw new NoVelocityException(); } // "Can't get velocity."
        public void SetLocation(Vector location) { _location = location; }
    }

    class MovableCantMove : IMovable  // MOC
    {
        private Vector _location;
        private Vector _velocity;
        public MovableCantMove(Vector location, Vector velocity)
        {
            _location = location;
            _velocity = velocity;
        }
        public Vector GetLocation() { return _location; }
        public Vector GetVelocity() { return _velocity; }
        public void SetLocation(Vector location) { throw new NoMovementException(); } // "Can't move."
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
    
    
        
    class Tests
    {
        public static void run ()
        {
            
        }
//        private static void log (string testName, bool passed)
//        {
//        }

    }

//    interface UObject
//    {
//        object this[string key]
//        {
//            get;
//            set;
//        }
//    }

//    class MovableAdapter : IMovable
//    {
//        UObject _obj;
//        public MovableAdapter(UObject obj)
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
