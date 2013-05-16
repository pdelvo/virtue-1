using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Virtue.API;
using Virtue.API.VersionControl;

namespace Virtue
{
    public static class Setup
    {
        public static void Run(Configuration config, Program instance)
        {
            var pluginFiles = Directory.GetFiles("plugins", "*.dll");
            var descriptors = new List<PluginDescriptor>(pluginFiles.Select(f => PluginInformationGetter.GetInformation(Path.GetFileName(f))).Where(a => a != null).ToArray());
            while (descriptors.Any())
            {
                Console.WriteLine("The following plugins were found in the 'plugins' folder:");
                for (int i = 0; i < descriptors.Count; i++)
                    Console.WriteLine("[{0}] {1}: {2}", i, descriptors[i].FriendlyName, descriptors[i].Description);
                Console.WriteLine("[n]: None");
                Console.WriteLine("Would you like to install any plugins?");
                Console.WriteLine("Select a number:");
                Console.Write("> ");
                var selection = Console.ReadLine();
                if (selection == "n")
                    break;
                int index;
                if (int.TryParse(selection, out index))
                {
                    if (index < descriptors.Count)
                    {
                        Console.WriteLine("Installed '{0}'", descriptors[index].FriendlyName);
                        config.Plugins = config.Plugins.Concat(new[] { descriptors[index].BaseDll }).ToArray();
                        descriptors.RemoveAt(index);
                    }
                }
            }
            instance.LoadPlugins();

            // Install projects
            string input;
            do
            {
                Console.Write("Would you like to set up a project (yes/no)? ");
                input = Console.ReadLine();
                if (input == "yes")
                {
                    var project = SetUpProject(instance);
                }
            } while (input != "no");
        }

        private static Project SetUpProject(Program instance)
        {
            var project = new Project();

            Console.Write("Project name: ");
            project.Name = Console.ReadLine();
            var root = Path.Combine("projects", SanitizeFileName(project.Name));
            Console.Write("Local repository path (enter for default): ");
            project.LocalRepository = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(project.LocalRepository))
                project.LocalRepository = Path.Combine(root, "source");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            if (!Directory.Exists(project.LocalRepository))
                Directory.CreateDirectory(project.LocalRepository);

            var providers = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes()).Aggregate((a, b) =>
                a.Concat(b).ToArray()).Where(t => typeof(IVersionControlProvider).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(t => Activator.CreateInstance(t) as IVersionControlProvider).ToArray();
            IVersionControlProvider provider;

            while (true)
            {
                Console.WriteLine("The following version control providers are available:");
                for (int i = 0; i < providers.Length; i++)
                    Console.WriteLine("[{0}]: {1}", i, providers[i].FriendlyName);
                Console.Write("> ");
                var selection = Console.ReadLine();
                int index;
                if (int.TryParse(selection, out index))
                {
                    provider = providers[index];
                    break;
                }
            }

            project.VCSProvider = project.GetType().AssemblyQualifiedName;

            Console.WriteLine("Do I need any credentials to clone this repository (yes/no)?");
            Console.Write("> ");
            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "yes" && input != "no");
            if (input == "yes")
                provider.GetIdentity(instance, project);

            Console.WriteLine("Enter the URL to clone the repository from:");
            Console.Write("> ");
            project.RepositoryURL = Console.ReadLine();

            provider.Clone(project.RepositoryURL, project.LocalRepository);

            return project;
        }

        private static string SanitizeFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, "_");
        }
    }
}
