
d:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface ITypeMetadata
	{
        string Name { get; set; }
        string Namespace { get; }
        
        Type Type { get; set; }

        IEnumerable<IInterfaceMetadata> Interfaces { get; }
		IEnumerable<IMethodMetadata> Methods { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
        IEnumerable<IPropertyMetadata> GetAttributes(Type type);

        IMetadataFormatter GetFormatter();
        IPartCollection GetContainingPartCollection();
	}
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
