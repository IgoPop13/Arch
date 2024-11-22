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
using HomeWorkFour;


// IoC.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand((int)args[0],(string)args[1])).Execute();
// ICommand cmd = IoC.Resolve<ICommand>("Commands.Move", obj);


namespace HomeWorkFive
{
    class IoC
    {
        private static IDictionary<string, obj[]> dependence;
        public static Resolve<T>(object[] args)
        {
            object[] depArgs;
            if (args[0] == "IoC.Register")
            {
                depArgs = new object[args.length - 2];
                for(int i = 2; i < args.length; i++)
                {
                    depArgs[i - 2] = args[i];
                }
                dependence[args[1]] = (depArgs) => {};
            }
            else
            {
                depArgs = new object[args.length - 1];
                for(int i = 1; i < args.length; i++)
                {
                    depArgs[i - 2] = args[i];
                }
                return (T)dependence[args[0]](depArgs);
            }
        }
        static IoC()
        {
            dependence = new IDictionary<string, obj[]>();
        }
    }

    public RegisterDependences
    {
        // зарегистрировать Scope
        // зарегистрировать команды, брать параметры из Scope
    }

    public InitInctances
    {
    }
    IoC.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand((int)args[0],(string)args[1])).Execute();  

}
