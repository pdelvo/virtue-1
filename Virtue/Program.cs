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
                            if (i >= args.Length)
                            {
                                Console.WriteLine("Missing parameter for '--config'");
                                return;
                            }
                            instance.ConfigurationFilePath = args[++i];
                            break;
                        case "--log":
                            if (i >= args.Length)
                            {
                                Console.WriteLine("Missing parameter for '--log'");
                                return;
                            }
                            instance.LogFilePath = args[++i];
                            break;
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
            {
                Setup();
                return false;
            }
            else
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationFilePath));
                Log("Loaded configuration file '{0}'", ConfigurationFilePath);
            }
            if (Configuration.Plugins.Length == 0)
                Log("No plugins loaded.");
            else
                LoadPlugins();
            if (Configuration.Projects.Length == 0)
                Log("No projects loaded.");
            else
                LoadProjects();
            return true;
        }

        private void LoadProjects()
        {
            
        }

        private void LoadPlugins()
        {
            
        }

        private void Setup()
        {
            Log("No configuration file found.");
            Console.WriteLine("Would you like to run first-time setup (1), or generate an empty{0}configuration (2)?", Environment.NewLine);
            string choice;
            do
            {
                Console.Write("> ");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2");
            if (choice == "1")
            {
                // TODO
            }
            else
            {
                var config = new Configuration();
                File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
                Log("Blank configuration generated in {0}, edit and restart.", ConfigurationFilePath);
            }
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
    }
}
