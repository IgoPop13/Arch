using SmartLinks.Classes;
using SmartLinks.Interfaces;

namespace SmartLinks.Interfaces
{
    public interface IProxyCommand : ICommand
    {
        public void ExecuteProxy();
        public IProxyCommand NextCommand { set; }
    }
}
