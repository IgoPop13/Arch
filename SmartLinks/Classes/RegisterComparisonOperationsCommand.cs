using SmartLinks.Classes;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class RegisterComparisonOperationsCommand : ICommand
    {
        public RegisterComparisonOperationsCommand()
        {
        }

        public void Execute()
        {
            foreach (string operation in IoC.Resolve<string[]>("Comparison.Operations"))
            {
                IoC.Resolve<ICommand>(
                    "IoC.Register",
                    operation,
                    (object[] args) => { IoC.Resolve<bool>(string.Format("Comparison.{0}.{1}", operation + args[0].GetType().Name)); }
                ).Execute();
            }
        }
    }
}
