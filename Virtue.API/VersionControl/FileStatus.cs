using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API.VersionControl
{
    public enum FileStatus
    {
        Added,
        Removed,
        Renamed,
        Deleted,
        Untracked,
        Modified
    }
}
