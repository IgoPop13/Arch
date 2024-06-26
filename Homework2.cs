//В результате выполнения ДЗ будет получен код, отвечающий за движение объектов по игровому полю, устойчивый к появлению новых игровых объектов и дополнительных ограничений, накладываемых на это движение.

//Описание/Пошаговая инструкция выполнения домашнего задания:
//В далекой звездной системе встретились две флотилии космических кораблей. Корабли могут передвигаться по всему пространству звездной системы по прямой, поворачиваться против и по часовой стрелке, стрелять фотонными торпедами. Попадание фотонной торпеды в корабль выводит его из строя.
//От каждой флотилии в сражении принимают участие по три космических корабля.
//Победу в битве одерживает та флотилия, которая первой выведет из строя все корабли соперника.
//Управление флотилиями осуществляется игрокам компьютерными программами (то есть не с клавиатуры).
//Концептуально игра состоит из трех подсистем:

//Игровой сервер, где реализуется вся игровая логика.
//Player - консольное приложение, на котором отображается конкретная битва.
//Агент - приложение, которое запускает программу управления танками от имени игрока и отправляет управляющие команды на игровой сервер.
//Реализовать движение объектов на игровом поле в рамках подсистемы Игровой сервер.

//Критерии оценки:
//За выполнение каждого пункта, перечисленного ниже начисляются баллы:

//ДЗ сдано на проверку - 1 балл
//Оформлен pull/merge request на github/gitlab - 1 балл
//Настроен CI - 2 балла
//Прямолинейное равномерное движение без деформации.
//Само движение реализовано в виде отдельного класса - 1 балл.
//Для движущихся объектов определен интерфейс, устойчивый к появлению новых видов движущихся объектов - 1 балл
//Реализован тесты (1 балл за все):
//Для объекта, находящегося в точке (12, 5) и движущегося со скоростью (-7, 3) движение меняет положение объекта на (5, 8)
//Попытка сдвинуть объект, у которого невозможно прочитать положение в пространстве, приводит к ошибке
//Попытка сдвинуть объект, у которого невозможно прочитать значение мгновенной скорости, приводит к ошибке
//Попытка сдвинуть объект, у которого невозможно изменить положение в пространстве, приводит к ошибке

//Поворот объекта вокруг оси.
//Сам поворот реализован в виде отдельного класса - 1 балл
//Для поворачивающегося объекта определен интерфейс, устойчивый к появлению новых видов движущихся объектов - 1 балл
//Реализован тесты - 1 балл.
//Итого: 10 баллов
//Задание считается принятым, если набрано не менее 7 баллов.


using System;

namespace HomeWorkTwo
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
            return new Vector (a.x + b.x, a.y + b.y);
        }
    }

    interface IMovable
    {
        Vector GetLocation();
        Vector GetVelocity();
        void SetLocation(Vector newValue);
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
            _movable.SetLocation(
                Vector.Plus(_movable.GetLocation(), _movable.GetVelocity())
            );
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
        public static Angle Plus (int angle, int aVelocity)
        {
            return new Angle ((angle + aVelocity) % _n, _n);
        }
    }
    
    interface IRotable
    {
        int GetAnge();
        int GetAngularVelocity();
        void SetAngle(Angle newValue);
    }

    class Rotate
    {
        IRotable _rotable;
        public Rotate(IRotable rotable)
        {
            _rotable = rotable;
        }
        public void Execute()
        {
            _rotable.SetAngle(Angle.Plus(_rotable.GetAngle(), _rotable.GetAngularVelocity()));
        }
    }

    interface UObject
    {
        object this[string key]
        {
            get;
            set;
        }
    }

    class MovableAdapter : IMovable
    {
        UObject _obj;
        public MovableAdapter(UObject obj)
        {
            _obj = obj;
        }

        public Vector GetLocation()
        {
            return (Vector) _obj["Location"];
        }
        
        public Vector GetVelocity()
        {
            Angle angle = (Angle) _obj["Angle"];
            int velocity = (int) _obj["Velocity"];
            return new Vector(velocity * Math.Cos(angle.ToDouble()), velocity * Math.Sin(angle.ToDouble()));
        }
        
        public void SetLocation(Vector newValue)
        {
            _obj["Location"] = newValue;
        }
 
    }
    
    class MOC
    {
         
    }
    
    class Tests
    {
        private static void log (string testName, bool passed)
        {
            // запись в лог: время начала, время окончания, название теста, результат
        }

        public static void run ()
        {
            UObject obj;
            new Move(new MovableAdapter(obj));
        }
    }
}
