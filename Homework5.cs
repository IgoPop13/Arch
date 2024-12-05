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

// OK Указание: Если Ваш фреймворк допускает работу с многопоточным кодом, то для работы со скоупами используйте ThreadLocal контейнер.

// Критерии оценки:
// OK Интерфейс IoC устойчив к изменению требований. Оценка: 0 - 3 балла (0 - совсем не устойчив, 3 - преподаватель не смог построить ни одного контрпримера)
// OK IoC предоставляет ровно один метод для всех операций. 1 балл
// OK IoC предоставляет работу со скоупами для предотвращения сильной связности. 2 балла.
// OK Реализованы модульные тесты. 2 балла
// Реализованы многопоточные тесты. 2 балла

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using HomeWorkFour;
using Xunit;

namespace HomeWorkFive
{
    public interface IDependencyResolver
    {
        public object Resolve(string dependency, object[] args);
    }

    public class DependencyResolver : IDependencyResolver
    {
        IDictionary<string, Func<object[], object>> _dependencies;

        public DependencyResolver(object scope)
        {
            _dependencies = (IDictionary<string, Func<object[], object>>) scope;
        }

        public object Resolve(string dependency, object[] args)
        {
            var dependencies = _dependencies;

            Func<object[], object>? dependencyResolverStrategy = null;
            if (dependencies.TryGetValue(dependency, out dependencyResolverStrategy))
            {
                return dependencyResolverStrategy(args);
            }
            else
            {
                return dependencyResolverStrategy;
            }
        }
    }

    // initiates thread, scope and IoC
    public class InitCommand : ICommand
    {
        internal static ThreadLocal<object> currentScope = new ThreadLocal<object>(true);

        static ConcurrentDictionary<string, Func<object[], object>> rootScope = new ConcurrentDictionary<string, Func<object[], object>>();
        
        public InitCommand()
        {
        }
        
        public void Execute()
        {
            lock (rootScope)
            {
                rootScope.TryAdd("IoC.Scope.Current.Set", (object[] args) => new SetCurrentScopeCommand(args[0]));
                rootScope.TryAdd("IoC.Scope.Current", (object[] args) => currentScope.Value != null ? currentScope.Value! : rootScope);
                rootScope.TryAdd("IoC.Scope.Create.Empty", (object[] args) => new Dictionary<string, Func<object[], object>>());
                rootScope.TryAdd("IoC.Scope.Create", (object[] args) => { return creatingScope = IoC.Resolve<IDictionary<string, Func<object[], object>>>("IoC.Scope.Create.Empty"); } );
                rootScope.TryAdd("IoC.Register", (object[] args) => new RegisterGameDependenciesCommand((string)args[0], (Func<object[], object>)args[1]));

                IoC.Resolve<ICommand>(
                    "IoC.ChangeDependencyResolveStrategy",
                    (Func<string, object[], object> oldStrategy) =>
                        (string dependency, object[] args) =>
                        {
                            var scope = currentScope.Value != null ? currentScope.Value! : rootScope;
                            var dependencyResolver = new DependencyResolver(scope);

                            return dependencyResolver.Resolve(dependency, args);
                        }
                ).Execute();
            }
        }
    }

    public class SetCurrentScopeCommand : ICommand
    {
        object _scope;
        public SetCurrentScopeCommand(object scope) { _scope = scope; }
        public void Execute() { InitCommand.currentScope.Value = _scope; }
    }

    public class RegisterDependencyCommand : ICommand
    {
        string _dependency;
        Func<object[], object> _dependencyResolverStratgey;

        public RegisterDependencyCommand(string dependency, Func<object[], object> depednecyResolverStrategy)
        {
            _dependency = dependency;
            _dependencyResolverStratgey = depednecyResolverStrategy;
        }

        public void Execute()
        {
            var currentScope = IoC.Resolve<IDictionary<string, Func<object[], object>>>("IoC.Scope.Current");
            currentScope.Add(_dependency, _dependencyResolverStratgey);
        }
    }

    private class UpdateIocResolveDependencyStrategyCommand : ICommand
    {
        Func<Func<string, object[], object>, Func<string, object[], object>> _updateIoCStrategy;

        public UpdateIocResolveDependencyStrategyCommand(
            Func<Func<string, object[], object>, Func<string, object[], object>> updater
        )
        {
            _updateIoCStrategy = updater;
        }

        public void Execute()
        {
            IoC._strategy = _updateIoCStrategy(IoC._strategy);
        }
    }

    public class IoC
    {
        internal static Func<string, object[], object> _strategy =
        (string dependency, object[] args) =>
        {
            if ("IoC.ChangeDependencyResolveStrategy" == dependency)
            {
                return new UpdateIocResolveDependencyStrategyCommand(
                    (Func<Func<string, object[], object>, Func<string, object[], object>>)args[0]
                );
            }
            else
            {
                throw new ArgumentException(@"Dependency {dependency} is not found.");
            }

        };

        public static T Resolve<T>(string dependency, params object[] args)
        {
            return (T)_strategy(dependency, args);
        }
    }

    // вынести в отдельный плагин, в котором прописать доступные объекты
    public class RegisterGameDependenciesCommand : ICommand
    {
        public RegisterGameDependenciesCommand()
        {
        }
        public void Execute()
        {
            (new InitCommand()).Execute();

            IoC.Resolve<ICommand>("IoC.Register", "Vector", (object[] args) => new Vector((int)args[0], (int)args[1])).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand((Vector)args[0], (Vector)args[1])).Execute();
            // зарегистрировать команды, брать параметры из Scope
        }
    }

    // вынести в отдельный модуль или организовать получение данных с клиента для инициализации игровых объектов
    public class InitGameInstancesCommand : ICommand
    {
        public InitGameInstancesCommand()
        {
        }
        public void Execute()
        {
            MoveCommand moveCommand = IoC.Resolve("Commands.Move", IoC.Resolve<Vector>("Vector", 0, 0), IoC.Resolve<Vector>("Vector", (1, 2)));
        }
    }

    public class Game : ICommand
    {
        public Game()
        {
        }
        public void Execute()
        {
            new RegisterGameDependenciesCommand().Execute();
            new InitGameInstancesCommand().Execuyte();
        }
    }

    public class UTests
    {
        [Fact]
        public void IoCThrowsArgumentExceptionForUnknownDependency()
        {
            Assert.Throws<ArgumentException>(() => Ioc.Resolve<object>("UnknownDependency"));
        }

        [Fact]
        public void IoCThrowsInvalidCastExceptionForDependencyWrongType()
        {
            Assert.Throws<InvalidCastException>(() => Ioc.Resolve<string>("IoC.ChangeDependencyResolveStrategy", (Func<string, object[], object> args) => args));
        }

        [Fact]
        public void IoCUpdatesResolveDependencyStrategy()
        {
            bool dependencyResolvedSuccessfully = false;

            Ioc.Resolve<ICommand>(
                "IoC.ChangeDependencyResolveStrategy",
                (Func<string, object[], object> args) =>
                {
                    dependencyResolvedSuccessfully = true;
                    return args;
                }
            ).Execute();

            Assert.True(dependencyResolvedSuccessfully);
        }
    }
}
