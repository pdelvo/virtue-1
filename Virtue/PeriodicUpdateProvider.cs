using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtue.API;

namespace Virtue
{
    public class PeriodicUpdateProvider : IUpdateProvider
    {
        public string FriendlyName 
        {
            get { return "Periodic Update"; } 
        }

        public void Setup(IPluginHost host, IProject project)
        {
            Console.WriteLine("Please specify the time (in minutes) between updates:");
            int minutes = -1;
            while (minutes <= 0)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                int.TryParse(input, out minutes);
            }
            project["PeriodicUpdateProvider.TimeBetweenUpdates"] = minutes;
        }

        public void Initialize(IPluginHost host, IProject project)
        {
            throw new NotImplementedException();
        }
    }
}
