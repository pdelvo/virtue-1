using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API
{
    public interface IPlugin
    {
        /// <summary>
        /// Runs when loading the plugin each session.
        /// </summary>
        void OnInitialize();

        /// <summary>
        /// Runs when the application shuts down.
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// Runs before OnInitialize on the first run after this plugin is installed.
        /// </summary>
        void OnInstalled();
    }
}
