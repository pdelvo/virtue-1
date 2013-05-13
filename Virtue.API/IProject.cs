using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API
{
    public interface IProject
    {
        string Name { get; set; }
        string LocalRepository { get; set; }
        string VCSProvider { get; set; }
        string RepositoryURL { get; set; }

        object this[string key] { get; set; }
    }
}
