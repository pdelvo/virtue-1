using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtue.API.VersionControl;

namespace Virtue.GitPlugin
{
    public class ModifiedFile : IModifiedFile
    {
        public ModifiedFile(string path, FileStatus status)
        {
            Path = path;
            Status = status;
        }

        public string Path { get; private set; }

        public FileStatus Status { get; private set; }
    }
}
