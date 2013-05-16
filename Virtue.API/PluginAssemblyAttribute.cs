using System;

namespace Virtue.API
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class PluginAssemblyAttribute : Attribute
    {
        public string FriendlyName { get; private set; }

        public string Version { get; private set; }

        public string Description { get; private set; }

        public PluginAssemblyAttribute(string friendlyName, string version, string description)
        {
            FriendlyName = friendlyName;
            Version = version;
            Description = description;
        }
    }
}
