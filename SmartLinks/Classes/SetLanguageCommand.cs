using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartLinks.Interfaces;
using System.Net;

namespace SmartLinks.Classes
{
    public class SetLanguageCommand : IProxyCommand
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

        public SetLanguageCommand(IDictionary<string, object>? args, IProxyCommand? nextCommand)
        {
            _nextCommand = nextCommand;
            _args = args;
        }

        public void Execute()
        {
            HttpContext context = IoC.Resolve<HttpContext>("HttpContext");

            string lang = context.Request.Headers["Accept-Language"].ToString().Substring(0, 2);
            List<string> languages = (List<string>)_args["supportedLanguages"];
            languages.Insert(0, lang);

            if (!IoC.Resolve<Func<object[], bool>>("Comparison.in.string")(languages.ToArray()))
                lang = (string)_args["defaultLanguage"];

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Language",
                (object[] args) => lang
            ).Execute();

            ExecuteProxy();
        }

        public void ExecuteProxy()
        {
            _nextCommand?.Execute();
        }
    }
}
