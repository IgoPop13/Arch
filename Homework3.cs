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
// OK 8. С помощью Команд из пункта 4 и пункта 6 реализовать следующую обработку исключений: при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.
// 9. Реализовать стратегию обработки исключения - повторить два раза, потом записать в лог. Указание: создать новую команду, точно такую же как в пункте 6. Тип этой команды будет показывать, что Команду не удалось выполнить два раза.

// Критерии оценки:
// OK ДЗ сдано на оценку - 2 балла
// OK Реализованы пункты 4-7. - 2 балла.
// OK Написаны тесты к пункту 4-7. - 2 балла
// OK Реализован пункт 8. - 1 балл
// OK Написаны тесты к пункту 8. - 1 балл
// Реализован пункт 9. - 1 балл
// Написаны тесты к пункту 9. - 1 балл
// Максимальная оценка за задание 10 баллов.
// Задание принимается, если задание оценено не менее, чем в 7 баллов.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace HomeWorkThree
{
    interface ICommand
    {
        public void Execute();
    }
   
    class Run
    {
        private void Init()
        {
            // Handler search tree MOC

            ExceptionHandler.RegisterHandler(typeof(FirstTimeCommand), typeof(FirstTimeException), (ICommand c, Exception e) => { return new FirstTimeCommandExceptionHandlerCommand(c, e); });
            ExceptionHandler.RegisterHandler(typeof(CommandToRepeat), typeof(RepeatedException), (ICommand c, Exception e) => { return new RepeatedCommandExceptionHandlerCommand(c, e); });
        }

        void ExecTests()
        {
            int TestPoint = 4; // DEFINE TEST POINT #
            Queue<ICommand> q = new Queue<ICommand>();
            ICommand c;


            switch(TestPoint)
            {
                case 4:
                case 5:
                    ExceptionHandler.RegisterHandler(typeof(FirstTimeCommand), typeof(FirstTimeException), (ICommand c, Exception e) => { return new FirstTimeCommandExceptionHandlerCommand(c, e); });
                    q.Enqueue(new FirstTimeCommand());
                    break;
                case 6:
                    ExceptionHandler.RegisterHandler(typeof(CommandToRepeat), typeof(RepeatedException), (ICommand c, Exception e) => { return new RepeatedCommandExceptionHandlerCommand(c, e); });
                    q.Enqueue(new CommandToRepeat());
                    break;
                case 7:
                    ExceptionHandler.RegisterHandler(typeof(CommandToRepeat), typeof(RepeatedException), (ICommand c, Exception e) => { return new QueueRepeatedCommandExceptionHandlerCommand(c, e); });
                    q.Enqueue(new CommandToRepeat());
                    break;
                case 8:
                    ExceptionHandler.RegisterHandler(typeof(Point8Command), typeof(Point8Exception), (ICommand c, Exception e) => { return new Point8Handler(c, e); });
                    q.Enqueue(new Point8Command());
                    break;
            }

            while(q.Count > 0)
            {
                c = q.Dequeue();
// POINT 1
                try
                {
                    c.Execute();
                }
// POINT 2
                catch (Exception ex)
                {
                    ex.Data.Add("Queue", q);
                    ExceptionHandler.Handle(c, ex).Execute();
                }
            }
        }
    }

// POINT 3
    static class ExceptionHandler
    {
        public static ICommand Handle(ICommand c, Exception e)
        {
            Type ct = c.GetType();
            Type et = e.GetType();

            return store[ct][et](c, e);
        }

        private static IDictionary <Type, IDictionary <Type, Func<ICommand, Exception, ICommand>>> store;

        public static void RegisterHandler(Type ct, Type et, Func<ICommand, Exception, ICommand> h)
        {
            if store == null
            {
                store = new IDictionary <Type, IDictionary <Type, Func<ICommand, Exception, ICommand>>>();
            }
            store[ct][et] = h;
        }
    }

    class FirstTimeException : Exception {}
    class RepeatedException : Exception{}

// POINT 8
    class Point8Exception : Exception{}

    class Point8Command : ICommand
    {
        public Point8Command()
        {

        }
        public void Execute()
        {
            (new LogCommand(this, new Exception ("NO EXCEPTION"))).Execute(); // LOG: Point8Command, NO EXCEPTION - TEST OF POINT 8
            throw new Point8Exception();
        }
    }

    class Point8Handler : ICommand
    {
        private ICommand _c;
        private Exception _e;

        public Point8Handler(ICommand c, Exception e) // ACTUALLY E IS NOT NEEDED HERE
        {
            _c = c;
            _e = e;
        }
        public void Execute()
        {
            try
            {
                (new RepeatedCommandExceptionHandlerCommand(_c, _e)).Execute();
            }
            catch(Exception ex)
            {
                (new LogCommand(_c, ex)).Execute();
            }
        }
    }

    class CommandToRepeat : ICommand
    {
        public CommandToRepeat()
        {
        }

        public void Execute()
        {
            (new LogCommand(this, new Exception ("NO EXCEPTION"))).Execute(); // LOG: CommandToRepeat, NO EXCEPTION - TEST OF POINT 6 OR POINT 7
            throw new RepeatedException();
        }
    }

    class FirstTimeCommand : ICommand
    {
        public FirstTimeCommand()
        {
        }
        public void Execute()
        {
            (new LogCommand(this, new Exception ("NO EXCEPTION"))).Execute(); // LOG: FirstTimeCommand, NO EXCEPTION - TEST OF POINT 4
            throw new FirstTimeException();
        }
    }

    class RepeatedCommandException : Exception {}

// POINT 4

    class LogCommand : ICommand
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

// POINT 5

    class FirstTimeCommandExceptionHandlerCommand : ICommand
    {
        private ICommand _c;
        private Exception _e;

        public FirstTimeCommandExceptionHandlerCommand(ICommand c, Exception e)
        {
            _c = c;
            _e = e;
        }
        public void Execute()
        {
            ((Queue<ICommand>)_e.Data["Queue"]).Enqueue(new LogCommand(_c, _e));
            (new LogCommand(this, new Exception ("FirstTimeException queued."))).Execute(); // LOG: FirstTimeCommand, Next record will be FirstTimeException - TEST OF POINT 5
        }
    }

// POINT 6

    class RepeatedCommandExceptionHandlerCommand : ICommand
    {
        private ICommand _c;
        public RepeatedCommandExceptionHandlerCommand(ICommand c, Exception e)
        {
            _c = c;
        }
        public void Execute()
        {
            _c.Execute();
        }
    }

// POINT 7

    class QueueRepeatedCommandExceptionHandlerCommand : ICommand
    {
        private ICommand _c;
        private Exception _e;
        public QueueRepeatedCommandExceptionHandlerCommand(ICommand c, Exception e)
        {
            _c = c;
            _e = e;
        }
        public void Execute()
        {
            ((Queue<ICommand>)_e.Data["Queue"]).Enqueue(new RepeatedCommandExceptionHandlerCommand(_c, _e));
        }
    }
}
