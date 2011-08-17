
d:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Collections;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IAgentPartMetadataCollection : IList<ITypeMetadata>
    {
        
    }

    public interface IAgentPartMetadataFormatterCollection : IMetadataFormatter, IAgentPartMetadataCollection
    {

		ITypeMetadata GetMetadata(Type agentPartImplementationType);
		bool Registered(Type agentPartImplementationType);
	}
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
