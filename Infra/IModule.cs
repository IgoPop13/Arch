using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLinks.Infra
{
    public interface IModule
    {
        /// <summary>
        /// Имя, которое будет отображаться на сайте
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Уникальное имя плагина
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Версия плагина
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Имя контроллера, который будет обрабатывать запросы
        /// </summary>
        string EntryControllerName { get; }
    }
}
