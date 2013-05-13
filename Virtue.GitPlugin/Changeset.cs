using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtue.API.VersionControl;

namespace Virtue.GitPlugin
{
    public class Changeset : IChangeset
    {
        public Changeset(IAuthor author, object identifier)
        {
            Author = author;
            Identifier = identifier;
        }

        public IAuthor Author { get; set; }

        public object Identifier { get; set; }
    }
}
