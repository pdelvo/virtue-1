using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API
{
    public interface IPlugin
    {
        /// <summary>
        /// The name of this plugin as displayed to users.
        /// </summary>
        string FriendlyName { get; }

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

        /// <summary>
        /// Called during project setup to offer plugins an opportunity to gather information from
        /// the user.
        /// </summary>
        void Setup(IProject project);
    }
}
