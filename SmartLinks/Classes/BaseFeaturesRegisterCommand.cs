using SmartLinks.Classes;
using SmartLinks.Interfaces;
using System.Numerics;

namespace SmartLinks.Classes
{
    public class BaseFeaturesRegisterCommand : ICommand
    {
        HttpContext _context;
        public BaseFeaturesRegisterCommand(HttpContext context)
        {
            _context = context;
        }

        private static DateTime Min(DateTime dt1, DateTime dt2)
        {
            if (dt1 > dt2)
                return dt2;
            else
                return dt1;
        }

        private static DateTime Max(DateTime dt1, DateTime dt2)
        {
            if (dt1 < dt2)
                return dt2;
            else
                return dt1;
        }

        public void Execute()
        {
            IoC.Resolve<ICommand>(
                "IoC.Register",
                "HttpContext",
                (object[] args) => _context
            ).Execute();

            // базовые операции - СКОРЕЕ ВСЕГО, НЕ НУЖНЫ - УБРАТЬ
            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Comparison.Operations",
                (object[] args) => { return new string[] { "gt", "gtEq", "lt", "ltEq", "eq", "in", "between", "notEq", "notIn", "beyond" }; }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "GetFeaturesListCommand",
                (object[] args) => new GetFeaturesListCommand()
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "FeaturesRegisterCommand",
                (object[] args) => new FeaturesRegisterCommand(IoC.Resolve<string[]>("FeaturesList"))
            ).Execute();

            // все базовые операции прописаны только для типа int, для остальных прописывается аналогично
            // либо - что предпочтительнее - можно определять тип arg[0] и приводить к нему все аргументы при сравнении, тогда зависимости для операций ниже будут универсальными для всех типов
            // НАЙТИ, КАК ДИНАМИЧЕСКИ ПРИВОДИТЬ object К ИСХОДНОМУ ТИПУ

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Comparison.in.string",
                (object[] args) =>
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        if ((string)args[i] == (string)args[0])
                            return true;
                    }
                    return false;
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Comparison.lt.DateTime",
                (object[] args) =>
                {
                    DateTime moment = (DateTime)args[0];
                    DateTime min;
                    if (args.Length > 2)
                        min = Min((DateTime)args[1], (DateTime)args[2]);
                    else
                        min = (DateTime)args[1];

                    return moment < min;
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Comparison.between.DateTime",
                (object[] args) =>
                {
                    DateTime moment = (DateTime)args[0];
                    DateTime min = Min((DateTime)args[1], (DateTime)args[2]);
                    DateTime max = Max((DateTime)args[1], (DateTime)args[2]);

                    return (moment >= min) & (moment <= max);
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Comparison.gt.DateTime",
                (object[] args) =>
                {
                    DateTime moment = (DateTime)args[0];
                    DateTime max;
                    if (args.Length > 2)
                        max = Max((DateTime)args[1], (DateTime)args[2]);
                    else
                        max = (DateTime)args[1];

                    return moment > max;
                }
            ).Execute();

            /*
                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.gt.int",
                            (object[] args) =>
                            {
                                return (int)args[0] > (int)args[1];
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.gtEq.int",
                            (object[] args) =>
                            {
                                return (int)args[0] >= (int)args[1];
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.lt.int",
                            (object[] args) =>
                            {
                                return (int)args[0] < (int)args[1];
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.ltEq.int",
                            (object[] args) =>
                            {
                                return (int)args[0] <= (int)args[1];
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.eq.int",
                            (object[] args) =>
                            {
                                return (int)args[0] == (int)args[1];
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.in.int",
                            (object[] args) =>
                            {
                                for (int i = 1; i < args.Length; i++)
                                {
                                    if ((int)args[i] == (int)args[0])
                                        return true;
                                }
                                return false;
                            }
                        ).Execute();

                        IoC.Resolve<ICommand>(
                            "IoC.Register",
                            "Comparison.between.int",
                            (object[] args) =>
                            {
                                if ((int)args[1] <= (int)args[2])
                                    return (int)args[0] >= (int)args[1] && (int)args[0] <= (int)args[2];
                                else
                                    return (int)args[0] >= (int)args[2] && (int)args[0] <= (int)args[1];
                            }
                        ).Execute();
            */
        }
    }
}
