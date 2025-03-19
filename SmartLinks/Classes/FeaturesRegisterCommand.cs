using SmartLinks.Interfaces;
using System.Numerics;

namespace SmartLinks.Classes
{
    public class FeaturesRegisterCommand : ICommand
    {
        private string[] _features;

        public FeaturesRegisterCommand(string[] features)
        {
            _features = features;
        }

        public void Execute()
        {
            // Здесь регистрируем зависимости, которые добавляются в IoC и используются при формировании цепочки ответственностей
            // При добавлении новых фич этот класс - единственный, который надо править, чтобы новые фичи разрешались через IoC

            // В принципе, все фичи можно регистрировать одним и тем же кодом с помощью кодогенерации, но я не разобрался, как исполнить сформированную строку.
            // Фрагменты ниже отличаются только ключом зависимости и названием объекта команды.

            if( _features == null )
                return;

            foreach (string feature in _features)
            {
                string dependencyRegisterTemplate = "IoC.Resolve<ICommand>(\"IoC.Register\", \"Event.{0}\", (object[] args) => new SetLanguageCommand((IDictionary<string, object>)args[0], (IProxyCommand)args[1])).Execute()";
                string dependencyRegistrationCode = string.Format(dependencyRegisterTemplate, feature);
                // TODO
                // выполнение сгенерированного кода
                // после реализации выполнения сгенерированного кода убрать регистрацию зависимостей хардкодом ниже
            }

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Event.SetLanguage",
                (object[] args) => new SetLanguageCommand((IDictionary<string, object>)args[0], (IProxyCommand)args[1])
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Event.ManageTime",
                (object[] args) => new ManageTimeCommand((IDictionary<string, object>)args[0], (IProxyCommand)args[1])
            ).Execute();

        }
    }
}
