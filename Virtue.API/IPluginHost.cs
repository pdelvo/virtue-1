using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API
{
    public interface IPluginHost
    {
        void Log(string text, params object[] parameters);
        void RunUpdate(IProject project);
        string ReadPassword();
    }
}
