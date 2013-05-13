using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtue.API.VersionControl;

namespace Virtue.GitPlugin
{
    public class ChangeAuthor : IAuthor
    {
        public ChangeAuthor(string name, string email)
        {
            Name = name;
            Email = email;
        }
        
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
