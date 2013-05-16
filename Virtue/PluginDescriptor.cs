using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue
{
    public class PluginDescriptor
    {
        public string FriendlyName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string BaseDll { get; set; }
    }
}
