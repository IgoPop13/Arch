using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class SetCurrentScopeCommand : ICommand
    {
        object _scope;
        public SetCurrentScopeCommand(object scope) { _scope = scope; }
        public void Execute() { InitCommand.currentScope.Value = _scope; }
    }
}
