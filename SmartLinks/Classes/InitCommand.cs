using System.Collections.Concurrent;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
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
                rootScope.TryAdd("IoC.Scope.Create", (object[] args) => { return IoC.Resolve<IDictionary<string, Func<object[], object>>>("IoC.Scope.Create.Empty"); });
                rootScope.TryAdd("IoC.Register", (object[] args) => new RegisterDependencyCommand((string)args[0], (Func<object[], object>)args[1]));

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
}
