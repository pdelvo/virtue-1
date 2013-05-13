using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtue.API
{
    public interface IUpdateProvider
    {
        string FriendlyName { get; }

        void Setup(IPluginHost host, IProject project);
        void Initialize(IPluginHost host, IProject project);
    }
}
