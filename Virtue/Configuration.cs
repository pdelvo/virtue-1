using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue
{
    public class Configuration
    {
        public Configuration()
        {
            Projects = new Project[0];
            Plugins = new string[0];
        }

        public Project[] Projects { get; set; }
        public string[] Plugins { get; set; }
    }
}
