// Домашнее задание
// Механизм обработки исключений в игре "Космическая битва"

// Цель: Научится писать различные стратегии обработки исключений так, чтобы соответствующий блок try-catсh не приходилось модифицировать каждый раз, когда возникает потребность в обработке исключительной ситуации по-новому.

// Описание/Пошаговая инструкция выполнения домашнего задания:
// Предположим, что все команды находятся в некоторой очереди. Обработка очереди заключается в чтении очередной команды и головы очереди и вызова метода Execute извлеченной команды. Метод Execute() может выбросить любое произвольное исключение.

// OK 1. Обернуть вызов Команды в блок try-catch.
// OK 2. Обработчик catch должен перехватывать только самое базовое исключение.
// 3. Есть множество различных обработчиков исключений. Выбор подходящего обработчика исключения делается на основе экземпляра перехваченного исключения и команды, которая выбросила исключение.
// 4. Реализовать Команду, которая записывает информацию о выброшенном исключении в лог.
// 5. Реализовать обработчик исключения, который ставит Команду, пишущую в лог в очередь Команд.
// 6. Реализовать Команду, которая повторяет Команду, выбросившую исключение.
// 7. Реализовать обработчик исключения, который ставит в очередь Команду - повторитель команды, выбросившей исключение.
// 8. С помощью Команд из пункта 4 и пункта 6 реализовать следующую обработку исключений: при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.
// 9. Реализовать стратегию обработки исключения - повторить два раза, потом записать в лог. Указание: создать новую команду, точно такую же как в пункте 6. Тип этой команды будет показывать, что Команду не удалось выполнить два раза.

// Критерии оценки:
// ДЗ сдано на оценку - 2 балла
// Реализованы пункты 4-7. - 2 балла.
// Написаны тесты к пункту 4-7. - 2 балла
// Реализован пункт 8. - 1 балл
// Написаны тесты к пункту 8. - 1 балл
// Реализован пункт 9. - 1 балл
// Написаны тесты к пункту 9. - 1 балл
// Максимальная оценка за задание 10 баллов.
// Задание принимается, если задание оценено не менее, чем в 7 баллов.



using System;
using System.Collections;
using System.Collections.Specialized;

namespace HomeWorkThree
{
    class RunTest
    {
        void Exec()
        {
            Rotate rotate;
            Move move;
            rotate = new Rotate (new Rotable (new Angle(0, 16), 3));

            Queue<ICommand> q = new Queue();

            // correct
            move = new Move (new Movable(new Vector(12, 5), new Vector(-7, 3)));
            q.Enqueue(move);
            
            // exceptions
            move = new Move (new MovableNoLocation(new Vector(12, 5), new Vector(-7, 3)));
            q.Enqueue(move);
            move = new Move (new MovableNoVelocity(new Vector(12, 5), new Vector(-7, 3)));
            q.Enqueue(move);
            move = new Move (new MovableCantMove(new Vector(12, 5), new Vector(-7, 3)));
            q.Enqueue(move);

            ICommand c;
            while(q.Count > 0)
            {
                c = q.Deque();
                try
                {
                    c.Execute();
                }
                catch (Exception e)
                {
                    ExceptionHandler.Handle(c, e, q).Execute();
                }
            }
        }
    }

    class ExceptionHandler
    {
        private NameValueCollection CommandsCollection;
        public ExceptionHandler()
        {
            // Handler search tree MOC
            CommandsCollection = new NameValueCollection();
            NameValueCollection ExceptionsCollection = new NameValueCollection();
            ExceptionsCollection.Add(NoLocationException.GetType(), "NoLocationException.Handler()"); // Subject to correct
            ExceptionsCollection.Add(NoVelocityException.GetType(), "NoVelocityException.Handler()"); // Subject to correct
            ExceptionsCollection.Add(NoMovementException.GetType(), "NoMovementException.Handler()"); // Subject to correct
            CommandsCollection.Add(Move.GetType(), ExceptionsCollection);
        }

        public static ICommand Handle(ICommand.c, Exception e, Queue q)
        {
            Type ct = c.GetType();
            Type et = e.GetType();
            // Write ct and et
        }
    }

    class LogCommand : ICommand
    {
        private ICommand _c
        private Exception _e
        public LogCommand(ICommand c, Exception e)
        {
            _c = c;
            _e = e;
        }
        public void Execute()
        {
        }
    }

    class Log : ICommand
    {
        public Log(ICommand c, Exception e)
        {
        }
        public void Execute()
        {
            // Write to log
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

    interface ICommand
    {
        public void Execute();
    }
    
    class Move : ICommand
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

    class NoLocationException : Exception {}
    class NoVelocityException : Exception {}
    class NoMovementException : Exception {}

    // MOCs
    class MovableNoVelocity : IMovable // MOC
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
}
