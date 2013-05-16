using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Virtue.API;

namespace Virtue
{
    public class Program : IPluginHost
    {
        public Configuration Configuration { get; set; }
        public string ConfigurationFilePath { get; private set; }
        public string LogFilePath { get; private set; }

        private StreamWriter LogStream { get; set; }

        static void Main(string[] args)
        {
            var instance = new Program();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.StartsWith("-"))
                {
                    switch (arg)
                    {
                        case "--config":
                            if (i >= args.Length - 1)
                            {
                                Console.WriteLine("Missing parameter for '--config'");
                                return;
                            }
                            instance.ConfigurationFilePath = args[++i];
                            break;
                        case "--log":
                            if (i >= args.Length - 1)
                            {
                                Console.WriteLine("Missing parameter for '--log'");
                                return;
                            }
                            instance.LogFilePath = args[++i];
                            break;
                        case "--generate-descriptor":
                            if (i >= args.Length - 1)
                            {
                                Console.WriteLine("Missing parameter for '--generate-descriptor'");
                                return;
                            }
                            GeneratePluginDescriptor(args[++i]);
                            return;
                        default:
                            Console.WriteLine("Invalid option '{0}'", arg);
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option '{0}'", arg);
                    return;
                }
            }

            instance.OpenLog();

            if (!instance.LoadConfiguration())
                return;

            instance.Run();
        }

        public Program()
        {
            ConfigurationFilePath = "config.json";
            LogFilePath = "log.txt";
        }

        private static void GeneratePluginDescriptor(string file)
        {
            var descriptor = new PluginDescriptor();
            Console.Write("Friendly name: ");
            descriptor.FriendlyName = Console.ReadLine();
            Console.Write("Version: ");
            descriptor.Version = Console.ReadLine();
            Console.Write("Description: ");
            descriptor.Description = Console.ReadLine();
            Console.Write("Base DLL: ");
            descriptor.BaseDll = Console.ReadLine();
            File.WriteAllText(file, JsonConvert.SerializeObject(descriptor));
            Console.WriteLine("Plugin descriptor '{0}' generated.", file);
        }

        private void Run()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            Log("Started Virtue {0}.{1} on {2} at {3}", version.Major, version.Minor, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            Console.WriteLine("Type 'help' for assistance, or 'quit' to exit.");
            string command;
            do
            {
                Console.Write("> ");
                command = Console.ReadLine();
            }
            while (command != "quit");
            if (LogStream != null)
                LogStream.Close();
        }

        private void OpenLog()
        {
            try
            {
                LogStream = new StreamWriter(LogFilePath, true, Encoding.UTF8);
                Log("Opened log file '{0}'", LogFilePath);
            }
            catch (Exception e)
            {
                Log("Failed to open log file {0} with following exception:", LogFilePath);
                Log(e.ToString());
            }
        }

        private bool LoadConfiguration()
        {
            if (!File.Exists(ConfigurationFilePath))
                return DoSetup();
            else
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationFilePath));
                Log("Loaded configuration file '{0}'", ConfigurationFilePath);
                if (Configuration.Plugins.Length == 0)
                    Log("No plugins loaded.");
                else
                    LoadPlugins();
                if (Configuration.Projects.Length == 0)
                    Log("No projects loaded.");
                else
                    LoadProjects();
            }
            return true;
        }

        internal void LoadProjects()
        {
            
        }

        internal void LoadPlugins()
        {
            foreach (var plugin in Configuration.Plugins)
            {
                var assembly = Assembly.LoadFrom(Path.Combine("plugins", plugin));

                    var attribute =
                        assembly.GetCustomAttributes(typeof (PluginAssemblyAttribute), false).SingleOrDefault() as PluginAssemblyAttribute;
                if(attribute == null) throw new InvalidOperationException("The given plugin assembly does not contain a plugin descriptor");

                var descriptor = new PluginDescriptor
                    {
                        BaseDll = plugin,
                        Description = attribute.Description,
                        FriendlyName = attribute.FriendlyName,
                        Version = attribute.Version
                    };

                Log("Loaded plugin '{0}'", descriptor.FriendlyName);
            }
        }

        private bool DoSetup()
        {
            GenerateDirectories();
            Log("No configuration file found.");
            Console.WriteLine("Would you like to run first-time setup (1), or generate an empty{0}configuration (2)?", Environment.NewLine);
            string choice;
            do
            {
                Console.Write("> ");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2");
            Configuration = new Configuration();
            if (choice == "1")
            {
                Setup.Run(Configuration, this);
                File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
                Log("Configuration saved to '{0}'", ConfigurationFilePath);
                return true;
            }
            else
            {
                File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
                Log("Blank configuration generated in {0}, edit and restart.", ConfigurationFilePath);
                return false;
            }
        }

        private void GenerateDirectories()
        {
            if (!Directory.Exists("plugins"))
                Directory.CreateDirectory("plugins");
            if (!Directory.Exists("projects"))
                Directory.CreateDirectory("projects");
        }

        public void Log(string text, params object[] parameters)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), string.Format(text, parameters));
            if (LogStream != null)
            {
                LogStream.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), string.Format(text, parameters));
                LogStream.Flush();
            }
        }


        public void RunUpdate(IProject project)
        {
            throw new NotImplementedException();
        }

        public string ReadPassword()
        {
            var passbits = new Stack<string>();
            for (ConsoleKeyInfo cki = Console.ReadKey(true); cki.Key != ConsoleKey.Enter; cki = Console.ReadKey(true))
            {
                if (cki.Key == ConsoleKey.Backspace)
                    passbits.Pop();
                else
                    passbits.Push(cki.KeyChar.ToString());
            }
            string[] pass = passbits.ToArray();
            Array.Reverse(pass);
            Console.Write(Environment.NewLine);
            return string.Join(string.Empty, pass);
        }
    }
}
