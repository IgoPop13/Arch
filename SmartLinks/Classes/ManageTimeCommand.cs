using Microsoft.Extensions.Logging;
using SmartLinks.Interfaces;

namespace SmartLinks.Classes
{
    public class ManageTimeCommand : IProxyCommand
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
        
        public ManageTimeCommand(IDictionary<string, object>? args, IProxyCommand? nextCommand)
        {
            this._nextCommand = nextCommand;
            _args = args;
        }

        public void Execute()
        {
            Dictionary<string, object> eventObj = IoC.Resolve<Dictionary<string, object>>("Event." + IoC.Resolve<int>("Event.Current.ID"));

            DateTime eventStart = Convert.ToDateTime(eventObj["start"].ToString());
            DateTime eventEnd = Convert.ToDateTime(eventObj["end"].ToString());
            string period = _args["period"].ToString();

            string templateName = _args["templateName"].ToString();
            string redirectTo = _args["redirectTo"].ToString();

            HttpContext context = IoC.Resolve<HttpContext>("HttpContext");

            if (!IoC.Resolve<Func<object[], bool>>($"Comparison.{period}.DateTime")(new object[] { DateTime.Now, eventStart, eventEnd }))
            {
                if (redirectTo != string.Empty)
                    context.Response.Redirect(redirectTo);

                if (templateName != string.Empty)
                    context.Response.Redirect("/" + templateName);
            }

            ExecuteProxy();
        }

        public void ExecuteProxy()
        {
            _nextCommand?.Execute();
        }
    }
}

/*
string jsonString = """
{
  "event":
  {
    "id": "929a3bd5-ce6a-42fe-9e8d-c710592086aa",
    "start": "2025-03-12T17:00:00+03",
    "end": "2025-03-12T19:00:00+03",
    "sequence": [
      {
        "dependency": "Event.SetLanguage",
        "supportedLanguages": [ "ru", "by" ],
        "defaultLanguage": "en"
      },
      {
        "dependency": "ManageTime",
        "period": "lt",
        "redirectTo": "",
        "templateName": "before.cshtml"
      },
      {
        "dependency": "ManageTime",
        "period": "in",
        "redirectTo": "",
        "templateName": "during.cshtml"
      },
      {
        "dependency": "ManageTime",
        "period": "gt",
        "redirectTo": "timetable.cshtml",
        "templateName": "after.cshtml"
      }
    ]
  }
}
""";
*/
