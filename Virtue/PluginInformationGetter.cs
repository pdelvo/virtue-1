using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Virtue.API;

namespace Virtue
{
    internal static class PluginInformationGetter
    {
        public static PluginDescriptor GetInformation(string name)
        {
            var domain = AppDomain.CreateDomain("pluginDomain");

            var getter = new InformationGetter
            {
                PluginName = name
            };

            domain.DoCallBack(getter.DoCallback);

            var descriptor = (PluginDescriptor)domain.GetData("descriptor");
            AppDomain.Unload(domain);
            return descriptor;
        }

        [Serializable]
        class InformationGetter
        {
            public string PluginName { get; set; }

            public void DoCallback()
            {
                string pluginName = PluginName;

                var assembly = Assembly.LoadFrom(Path.Combine("plugins", pluginName));
                if(assembly == null) return;
                var attribute =
                    assembly.GetCustomAttributes(typeof(PluginAssemblyAttribute), false).SingleOrDefault() as PluginAssemblyAttribute;
                if (attribute == null) throw new InvalidOperationException("The given plugin assembly does not contain a plugin descriptor");

                var descriptor = new PluginDescriptor
                {
                    BaseDll = pluginName,
                    Description = attribute.Description,
                    FriendlyName = attribute.FriendlyName,
                    Version = attribute.Version
                };

                AppDomain.CurrentDomain.SetData("descriptor", descriptor);
            }
        }
    }
}
