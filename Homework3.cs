 // Домашнее задание
// Механизм обработки исключений в игре "Космическая битва"

// Цель: Научится писать различные стратегии обработки исключений так, чтобы соответствующий блок try-catсh не приходилось модифицировать каждый раз, когда возникает потребность в обработке исключительной ситуации по-новому.

// Описание/Пошаговая инструкция выполнения домашнего задания:
// Предположим, что все команды находятся в некоторой очереди. Обработка очереди заключается в чтении очередной команды и головы очереди и вызова метода Execute извлеченной команды. Метод Execute() может выбросить любое произвольное исключение.

// OK 1. Обернуть вызов Команды в блок try-catch.
// OK 2. Обработчик catch должен перехватывать только самое базовое исключение.
// OK 3. Есть множество различных обработчиков исключений. Выбор подходящего обработчика исключения делается на основе экземпляра перехваченного исключения и команды, которая выбросила исключение.
// OK 4. Реализовать Команду, которая записывает информацию о выброшенном исключении в лог.
// OK 5. Реализовать обработчик исключения, который ставит Команду, пишущую в лог в очередь Команд.
// OK 6. Реализовать Команду, которая повторяет Команду, выбросившую исключение.
// OK 7. Реализовать обработчик исключения, который ставит в очередь Команду - повторитель команды, выбросившей исключение.
// 8. С помощью Команд из пункта 4 и пункта 6 реализовать следующую обработку исключений: при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.
// 9. Реализовать стратегию обработки исключения - повторить два раза, потом записать в лог. Указание: создать новую команду, точно такую же как в пункте 6. Тип этой команды будет показывать, что Команду не удалось выполнить два раза.

// Критерии оценки:
// OK ДЗ сдано на оценку - 2 балла
// РOK еализованы пункты 4-7. - 2 балла.
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
            MoveCommand move;
            rotate = new Rotate (new Rotable (new Angle(0, 16), 3));

            Queue<ICommand> q = new Queue();

            // correct
            move = new MoveCommand (new Movable(new Vector(12, 5), new Vector(-7, 3))); // MOC OK
            q.Enqueue(move);
            
            // exceptions
            move = new NoLocationExceptionCommand (new MovableNoLocation(new Vector(12, 5), new Vector(-7, 3))); // MOC exception, need to be replaced with MoveCommand
            q.Enqueue(move);
            move = new NoVelocityExceptionCommand (new MovableNoVelocity(new Vector(12, 5), new Vector(-7, 3))); // MOC exception, need to be replaced with MoveCommand
            q.Enqueue(move);
            move = new NoMovementExceptionCommand (new MovableCantMove(new Vector(12, 5), new Vector(-7, 3))); // MOC exception, need to be replaced with MoveCommand
            q.Enqueue(move);

            ExceptionHandler eh = new ExceptionHandler();

            ICommand c;
            while(q.Count > 0)
            {
                c = q.Deque();
                try // point 1
                {
                    c.Execute();
                }
                catch (Exception e) // point 2
                {
                    eh.Handle(c, e, q).Execute();
                }
            }
        }
    }

    interface IMovable
    {
        public Vector GetLocation();
        public Vector GetVelocity();
        public void SetLocation(Vector newValue);
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
    
    interface ICommand
    {
        public void Execute();
    }

    class MoveCommand : ICommand
    {
        IMovable _movable;
        public MoveCommand(IMovable movable)
        {
            _movable = movable;
        }
        public void  Execute()
        {
            _movable.SetLocation(Vector.Plus(_movable.GetLocation(), _movable.GetVelocity()));
        }
    }
    
    class NoLocationException : Exception {}
    
    class NoLocationExceptionCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public NoLocationExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            q.Enqueue(_c); // point 6
        }
    }

    class NoVelocityException : Exception {}
 
    class NoVelocityExceptionCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public NoVelocityExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            q.Enqueue(new NoLocationExceptionCommand(_c, _e, _q)); // point 7
        }
    }

    class NoMovementException : Exception {}

    class NoMovementExceptionCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public NoMovementExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            q.Enqueue(new LogCommand(_c, _e, _q)); // point 5
        }
    }

    class LogCommand : ICommand // point 4
    {
        private ICommand _c;
        private Exception _e;
        public LogCommand(ICommand c, Exception e)
        {
            _c = c;
            _e = e;
        }
        public void Execute()
        {
            // Writeline("Command: {0}. Ecxeption: {1}", _c, _e);
        }
    }

    class RepeatCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public NoMovementExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            q.Enqueue(_c);
        }
    }

    class ExceptionHandler
    {
        private NameValueCollection CommandsCollection;
        public ExceptionHandler()
        {
            // Handler search tree MOC
            CommandsCollection = new NameValueCollection(); // point 3
            NameValueCollection MoveExceptionsCollection;
            
            MoveExceptionsCollection = new NameValueCollection();
            MoveExceptionsCollection.Add(NoLocationException.GetType(), NoLocationExceptionCommand); // NoLocationExceptionCommand is a MOC type, finally should be replaced with MoveCommand type
            CommandsCollection.Add(NoLocationExceptionCommand.GetType(), MoveExceptionsCollection); // NoLocationExceptionCommand is a MOC type, finally should be replaced with MoveCommand type
            
            MoveExceptionsCollection = new NameValueCollection();
            MoveExceptionsCollection.Add(NoVelocityException.GetType(), NoVelocityExceptionCommand); // NoVelocityExceptionCommand is a MOC type, finally should be replaced with MoveCommand type
            CommandsCollection.Add(NoVelocityExceptionCommand.GetType(), MoveExceptionsCollection); // NoVelocityExceptionCommand is a MOC type, finally should be replaced with MoveCommand type

            MoveExceptionsCollection = new NameValueCollection();
            MoveExceptionsCollection.Add(NoMovementException.GetType(), NoMovementExceptionCommand); // NoMovementExceptionCommand is a MOC type, finally should be replaced with MoveCommand type
            CommandsCollection.Add(NoMovementExceptionCommand.GetType(), MoveExceptionsCollection); // NoMovementExceptionCommand is a MOC type, finally should be replaced with MoveCommand type
            
            
            // CommandsCollection.Add(NoLocationExceptionCommand.GetType(), MoveExceptionsCollection);
            // RepeatedExceptionsCollection = new NameValueCollection();
            // RepeatedExceptionsCollection.Add(RepeatedCommandException, RepeatedCommandExceptionCommand);
            // CommandsCollection.Add(CommandRepeater, RepeatedExceptionsCollection);
            // TwiceRepeatedExceptionsCollection = new NameValueCollection();
            // TwiceRepeatedExceptionsCollection.Add(TwiceRepeatedCommandException, TwiceRepeatedCommandExceptionCommand);
            // CommandsCollection.Add(SecondCommandRepeater, TwiceRepeatedExceptionsCollection);
        }

        public ICommand Handle(ICommand c, Exception e, Queue q)
        {
            Type ct = c.GetType();
            Type et = e.GetType();
            return new CommandsCollection.GetValues(ct)[0].GetValues(et)[0](c, e, q);
        }
    }

    class SecondCommandRepeater : ICommand
    {
        private ICommand _c;
        public SecondCommandRepeater(ICommand c)
        {
            _c = c;
        }
        public Execute()
        {
            _c.Execute();
        }
    }

    class TwiceRepeatedCommandExceptionCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public TwiceRepeatedCommandExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            ICommand LogCommand = new LogCommand(c, e, q);
            LogCommand.Execute();
        }
    }
    
    class CommandRepeater : ICommand
    {
        private ICommand _c;
        public CommandRepeater(ICommand c)
        {
            _c = c;
        }
        public Execute()
        {
            _c.Execute();
        }
    }

    class RepeatedCommandExceptionCommand : ICommand
    {
        private Queue _q;
        private ICommand _c;
        private Exception _e;
        public RepeatedCommandExceptionCommand(ICommand c, Exception e, Queue q)
        {
            _q = q;
            _c = c;
            _e = e;
        }
        Execute()
        {
            ICommand LogCommand = new LogCommand(c, e, q);
            LogCommand.Execute();
        }
    }
    
    interface IRotable
    {
        public Angle GetAngle();
        public int GetAngularVelocity();
        public void SetAngle(Angle newValue);
    }

    class Rotate : ICommand
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

    class RepeatedCommandException : Exception {}
    class TwiceRepeatedCommandException : Exception {}

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
