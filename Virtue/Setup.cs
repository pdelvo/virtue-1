using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Virtue
{
    public static class Setup
    {
        public static void Run(Configuration config)
        {
            var pluginFiles = Directory.GetFiles("plugins", "*.json");
            var descriptors = new List<PluginDescriptor>(pluginFiles.Select(f =>
                JsonConvert.DeserializeObject<PluginDescriptor>(File.ReadAllText(f))).ToArray());
            while (true)
            {
                Console.WriteLine("The following plugins were found in the 'plugins' folder:");
                for (int i = 0; i < descriptors.Count; i++)
                    Console.WriteLine("[{0}] {1}: {2}", i, descriptors[i].FriendlyName, descriptors[i].Description);
                Console.WriteLine("[n]: None");
                Console.WriteLine("[r]: Refresh list");
                Console.WriteLine("Select a number:");
                Console.Write("> ");
                var selection = Console.ReadLine();
                if (selection == "n")
                    break;
                if (selection == "r")
                {
                    pluginFiles = Directory.GetFiles("plugins", "*.json");
                    descriptors = new List<PluginDescriptor>(pluginFiles.Select(f =>
                        JsonConvert.DeserializeObject<PluginDescriptor>(File.ReadAllText(f))).ToArray());
                }
                int index;
                if (int.TryParse(selection, out index))
                {
                    if (index < descriptors.Count)
                    {
                        Console.WriteLine("Installed '{0}'", descriptors[index].FriendlyName);
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadKey(true);
                        descriptors.RemoveAt(index);
                    }
                }
            }
        }
    }
}
