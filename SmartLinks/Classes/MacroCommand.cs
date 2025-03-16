using SmartLinks.Classes;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class MacroCommand : ICommand
    {
        List<ICommand> _commands;
        public MacroCommand(List<ICommand> commands)
        {
            _commands = commands;
        }

        public void Execute()
        {
            foreach (SmartLinks.Interfaces.ICommand command in _commands)
            {
                command.Execute();
            }
        }
    }
}
