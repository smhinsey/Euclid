using System;
using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IPartCollection 
    {
        string AgentSystemName { get; }
        string Namespace { get; }
        string DescriptiveName { get; }
        Type CollectionType { get; }

        IEnumerable<ITypeMetadata> Collection { get; }
        IMetadataFormatter GetFormatter();
    }
}