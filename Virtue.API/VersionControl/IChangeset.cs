﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API.VersionControl
{
    public interface IChangeset
    {
        IAuthor Author { get; set; }
        object Identifier { get; set; }
    }
}
