using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SmartLinks.Infra;

namespace SmartLinks.PluginManager
{
    public class PluginManager
    {
       public PluginManager()
        {
            Modules = new Dictionary<IModule, Assembly>();
        }

        private static PluginManager _current;
        public static PluginManager Current
        {
            get { return _current ?? (_current = new PluginManager()); }
        }

        internal Dictionary<IModule, Assembly> Modules { get; set; }
        //Возвращаем все загруженные модули
        public IEnumerable<IModule> GetModules()
        {
            return Modules.Select(m => m.Key).ToList();
        }
        //Получаем плагин по имени
        public IModule GetModule(string name)
        {
            return GetModules().Where(m => m.Name == name).FirstOrDefault();
        }
    }
}
