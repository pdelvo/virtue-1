using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API.VersionControl
{
    public interface IChangeset
    {
        public IAuthor Author { get; set; }
        public object Identifier { get; set; }
    }
}
