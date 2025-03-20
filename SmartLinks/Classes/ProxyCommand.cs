using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class ProxyCommand : IProxyCommand
    {
        private IProxyCommand? _nextCommand;
        private IDictionary<string, object>? _args;

        public IProxyCommand NextCommand
        {
            set
            {
                _nextCommand = value;
            }
        }

        public ProxyCommand(IDictionary<string, object>? arguments, IProxyCommand? nextCommand)
        {
            _nextCommand = nextCommand;
            _args = arguments;
        }

        public void Execute()
        {
            this.ExecuteProxy();
        }

        public void ExecuteProxy()
        {
            _nextCommand?.Execute();
        }
    }
}
