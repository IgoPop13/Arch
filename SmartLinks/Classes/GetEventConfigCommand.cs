using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class GetEventConfigCommand : ICommand
    {
        public GetEventConfigCommand()
        {
        }
        public void Execute()
        {
            HttpContext httpContext = IoC.Resolve<HttpContext>("HttpContext");

            string eventId = httpContext.Request.Query["event"]!;
            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Event.Current.ID",
                (object[] args) => eventId
            ).Execute();

            Dictionary<string, object> eventObj;

            try
            {
                eventObj = IoC.Resolve<Dictionary<string, object>>("Event." + eventId);
            }
            catch
            {
                eventObj = new Dictionary<string, object>();

                HttpClient httpClient = new HttpClient();

                string eventConfigJsonString = httpClient.GetStringAsync($"http://localhost/SmartlinksEditor/{eventId}.json").Result;

                // parse event config

                JsonDocument eventConfigJson = JsonDocument.Parse(eventConfigJsonString);

                JsonElement eventNode = eventConfigJson.RootElement.GetProperty("event");


                // по-хорошему, здесь должна быть рекурсивная функция, структура получится древовидной и для каждого узла/элеимента выполняются однотипные действия,
                // однако пришлось сделать так для экономии времени, для целей проекта этого достаточно
                foreach (JsonProperty node in eventNode.EnumerateObject())
                {
                    JsonElement element = eventNode.GetProperty(node.Name);
                    if (element.ValueKind != JsonValueKind.Array)
                    {
                        eventObj[node.Name] = node.Value;
                    }
                    else
                    {
                        eventObj[node.Name] = new Collection<Dictionary<string, object>>();
                        JsonElement arrayOfElements = node.Value;
                        Dictionary<string, object> unnamedObject = new Dictionary<string, object>();
                        foreach (JsonProperty unnamed in arrayOfElements.EnumerateObject())
                        {
                            foreach (JsonProperty unnamedObjectProperty in unnamed.Value.EnumerateObject())
                                unnamedObject[unnamedObjectProperty.Name] = unnamedObjectProperty.Value;

                            ((Collection<Dictionary<string, object>>)eventObj[node.Name]).Add(unnamedObject);
                        }
                    }
                }

                if (eventId != (string)eventObj["id"])
                    httpContext.Response.Redirect("/Pages/404.cshtml");

                Collection<Dictionary<string, object>> sequence = (Collection<Dictionary<string, object>>)eventObj["sequence"];

                int i = 0;
                IProxyCommand[] proxyCommands = new IProxyCommand[sequence.Count];
                foreach (Dictionary<string, object> dependency in sequence)
                {
                    proxyCommands[i++] = IoC.Resolve<IProxyCommand>((string)dependency["dependency"], dependency, null);
                }

                for (i = 0; i < proxyCommands.Length - 1; i++)
                {
                    proxyCommands[i].NextCommand = proxyCommands[i + 1];
                }

                eventObj["ResponsibilityChainStartCommand"] = proxyCommands[0];

                // Сохраняем объект настроек события в IoC - при просмотре нескольких событий в рамках одной сессии настройки каждого события будут запрошени лишь единожды.
                // Для сохранения подошёл бы и объект Session.

                IoC.Resolve<ICommand>(
                    "IoC.Register",
                    "Event." + eventId,
                    (object[] args) => eventObj
                ).Execute();
            }
        }
    }
}
