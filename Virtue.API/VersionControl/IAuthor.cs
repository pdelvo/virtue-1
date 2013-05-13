using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API.VersionControl
{
    public interface IAuthor
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
