using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows.Input;
using SmartLinks.Classes;
using SmartLinks.Interfaces;
using Microsoft.Extensions.Logging;

namespace SmartLinks.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            new MacroCommand
                (new List<SmartLinks.Interfaces.ICommand>
                    {
                        new InitCommand(),
                        new BaseFeaturesRegisterCommand(context),
                        IoC.Resolve<SmartLinks.Interfaces.ICommand>("GetFeaturesListCommand")
                    }
                ).Execute();

            new MacroCommand
                (new List<SmartLinks.Interfaces.ICommand>
                    {
                        IoC.Resolve<SmartLinks.Interfaces.ICommand>("FeaturesRegisterCommand"),
                        new GetEventConfigCommand()
                    }
                ).Execute();


            Dictionary<string, object> eventObj = IoC.Resolve<Dictionary<string, object>>("Event." + IoC.Resolve<string>("Event.Current.ID"));

            ((IProxyCommand)eventObj["ResponsibilityChainStartCommand"]).Execute();

            await _next(context);
        }
    }
}

