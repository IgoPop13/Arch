/*
Домашнее задание
Многопоточное выполнение команд

Цель:
Разработка многопоточной системы выполнения команд на примере игры "Космический бой".

В результате выполнения ДЗ будет получен код, отвечающий за выполнение множества команд в несколько потоков, устойчивый к появлению новых видов команд и дополнительных ограничений, накладываемых на них.


Описание/Пошаговая инструкция выполнения домашнего задания:
Предположим, что у нас есть набор команд, которые необходимо выполнить. Выполнение команд организуем в несколько потоков.
Для этого будем считать, что у каждого потока есть своя потокобезопасная очередь.
Для того, чтобы выполнить команду, ее необходимо добавить в очередь. Поток читает очередную команду из очереди и выполняет ее.
Если выполнение команды прерывается выброшенным исключением, то поток должен отловить его и продолжить работу.
Если сообщений нет в очереди, то поток засыпает до тех пор, пока очередь пуста.

Последовательность шагов решения:

OK 1. Реализовать код, который запускается в отдельном потоке и делает следующее: В цикле получает из потокобезопасной очереди команду и запускает ее. Выброс исключения из команды не должен прерывать выполнение потока.
OK 2. Написать команду, которая стартует код, написанный в пункте 1 в отдельном потоке.
OK 3. Написать команду, которая останавливает цикл выполнения команд из пункта 1, не дожидаясь их полного завершения (hard stop).
OK 4. Написать команду, которая останавливает цикл выполнения команд из пункта 1, только после того, как все команды завершат свою работу (soft stop).
5. Написать тесты на команду запуска и остановки потока.

Критерии оценки:
За выполнение каждого пункта, перечисленного ниже начисляются баллы:

OK ДЗ сдано на проверку - 2 балла
OK Код решения опубликован на github/gitlab - 1 балл
OK Настроен CI - 2 балла
OK Код компилируется без ошибок - 1 балл.
Написать тест, который проверяет, что после команды старт поток запущен - 1балл и 4 балла - если используются условные события синхронизации.
OK Написать тест, который проверяет, что после команды hard stop, поток завершается - 1 балл
OK Написать тест, который проверяет, что после команды soft stop, поток завершается только после того, как все задачи закончились - 2 балла
Итого: 10 баллов
Задание считается принятым, если набрано не менее 7 баллов.
*/

using System;
using Xunit;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using HomeWorkThree;

namespace HomeWorkSeven
{
    public class UTests
    {
        [Fact]
        public static void Asserts(ICommand c, GameThread gt, BlockingCollection<ICommand> q, int qLength)
        {
            Assert.WasCalled(c.Execute);
            Assert.WasCalled(gt.StopHook);
            Assert.AreEqual(q.Count, qLength);
        }
    }

    public class RunGame
    {
        private BlockingCollection<GameThread> _gameThreadCollection;

        public RunGame()
        {
            IoCInit();
            _gameThreadCollection = new BlockingCollection<GameThread>();
            AddThreads();
        }

        private void IoCInit()
        {
            IoC.Resolve<ICommand>("IoC.Register", $"Handler.{ExceptionCommand.GetType().Name}.{SomeException.GetType().Name}", (object[] args) => {
                new ExceptionHandler();
            }).Execute();
            IoC.Resolve<ICommand>("IoC.Register", $"Handler.{CommonCommand.GetType().Name}.{SomeException.GetType().Name}", (object[] args) => {
                new ExceptionHandler();
            }).Execute();
        }

        public void AddThreads()
        {
            // тест SoftStop
            BlockingCollection<ICommand> q1 = new BlockingCollection<ICommand>();
            GameThread gt1 = new GameThread(q1);
            _gameThreadCollection.Add(gt1);
            ICommand c1 = new CommonCommand();
            q.Add(c1);
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new ExceptionCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new SoftStopCommand(gt1));
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            (new RunNewThreadCommand(gt1)).Execute();

            // тест HardStop
            BlockingCollection<ICommand> q2 = new BlockingCollection<ICommand>();
            GameThread gt2 = new GameThread(q2);
            _gameThreadCollection.Add(gt2);
            ICommand c2 = new CommonCommand();
            q.Add(c2);
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new ExceptionCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new HardStopCommand(gt2));
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            q.Add(new CommonCommand());
            (new RunNewThreadCommand(gt2)).Execute();

            // в очереди должно остаться 0 комманд
            UTests.Asserts(c1, gt1, q1, 0);

            // в очереди должно остаться 5 команд
            UTests.Asserts(c2, gt2, q2, 5);
        }
    }

    public class RunNewThreadCommand : ICommand
    {
        GameThread _gt;

        public RunNewThreadCommand(GameThread gt)
        {
            _gt = gt;
        }
        public void Execute()
        {
            _gt.Start();
        }
    }

    public class GameThread
    {
        BlockingCollection<ICommand> _q;
        bool _stop;

        public bool Stop()
        {
            _stop = true;
        }

        public BlockingCollection<ICommand> Queue
        {
            get
            {
                return _q;
            }
        }

        public void StartHook()
        {
        }

        public void StopHook()
        {
        }

        public GameThread(BlockingCollection<ICommand> q)
        {
            _q = q;
        }

        public void Start()
        {
            _stop = false;

            ICommand cmd;

            Thread t = new Thread(
                () =>
                {
                    StartHook();
                    while (!stop)
                    {
                        cmd = q.Take();
                        try
                        {
                            cmd.Execute();
                        }
                        catch (Exception e)
                        {
                            IoC.Resolve<ICommand>($"Handler.{cmd.GetType().Name}.{e.GetType().Name}").Execute();
                        }
                    }
                    StopHook();
                }
            );
        }
    }

    public class HardStopCommand : ICommand
    {
        GameThread _t;
        public HardStopCommand(GameThread t)
        {
            _t = t;
        }
        
        public void Execute()
        {
            _t.Stop();
        }
    }

    public class SoftStopCommand : ICommand
    {
        GameThread _t;
        BlockingCollection<ICommand> _q;

        public SoftStopCommand(GameThread t)
        {
            _t = t;
        }
        
        public void Execute()
        {
            _q = _t.Queue;

            if (_q.Count == 0)
            {
                _t.Stop();
            }
            else
            {
                _q.Add(new SoftStopCommand(_t));
            }
        }
    }

    public class CommonCommand : ICommand
    {
        public CommonCommand()
        {
        }

        public void Execute()
        {
            // какие-то действия
        }
    }

    public class ExceptionCommand : ICommand
    {
        public ExceptionCommand()
        {
        }
        
        public void Execute()
        {
            // какие-то действия
            throw new SomeException();
        }
    }

    public class SomeException : Exception
    {
    }

    public class ExceptionHandler : ICommand
    {
        public ExceptionHandler()
        {
            
        }
        public void Execute()
        {
            // обработка исключения
        }
    }
}
