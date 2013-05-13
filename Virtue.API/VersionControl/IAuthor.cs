using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API.VersionControl
{
    public interface IAuthor
    {
        string Name { get; set; }
        string Email { get; set; }
    }
}
