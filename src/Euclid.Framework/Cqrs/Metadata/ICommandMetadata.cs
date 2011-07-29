
D:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface ICommandMetadata
    {
        string Name { get; set; }
        Type Type { get; set; }
        string Namespace { get; }
        IList<IPropertyMetadata> Properties { get; }
        IList<IMetadata> Interfaces { get; }
    }
}
D:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

D:\Projects\Euclid\platform>@rem Restore the original console codepage.

D:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
