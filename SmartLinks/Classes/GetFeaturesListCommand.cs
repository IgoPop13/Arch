using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class GetFeaturesListCommand : ICommand
    {
        public GetFeaturesListCommand()
        {
        }

        public void Execute()
        {
            IoC.Resolve<ICommand>(
                "IoC.Register",
                "FeaturesList",
                (object[] args) => JsonDocument.Parse(new HttpClient().GetStringAsync("http://localhost/SmartlinksEditor/FeaturesList.json").Result).RootElement.GetProperty("featuresList").Deserialize<string[]>()
            ).Execute();
        }
    }
}
