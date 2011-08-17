using System;
using System.Reflection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Framework.Agent.Metadata
{

    public interface IAgentMetadata : IMetadataFormatter
    {
        string DescriptiveName { get; }
        string SystemName { get; }
        bool IsValid { get; }
        
        Assembly AgentAssembly { get; }
		IPartCollection Commands { get; }
		IPartCollection Queries { get; }
		IPartCollection ReadModels { get; }

        string GetBasicRepresentation(string format);

        IPartCollection GetPartCollectionContainingType(Type partType);
        IPartCollection GetPartCollectionContainingPartName(string partName);
        IPartCollection GetPartCollectionByDescriptiveName(string descriptiveName);

        ITypeMetadata GetPartByTypeName(string partName);
    }
}