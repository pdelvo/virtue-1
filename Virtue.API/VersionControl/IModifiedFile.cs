using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtue.API.VersionControl
{
    public interface IModifiedFile
    {
        string Path { get; }
        FileStatus Status { get; }
    }
}
