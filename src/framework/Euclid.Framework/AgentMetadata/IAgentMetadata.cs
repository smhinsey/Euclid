using System;
using System.Reflection;

namespace Euclid.Framework.AgentMetadata
{
	public interface IAgentMetadata : IMetadataFormatter
	{
		Assembly AgentAssembly { get; }
		IPartCollection Commands { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }
		IPartCollection Queries { get; }
		IPartCollection ReadModels { get; }
		string SystemName { get; }

		string GetBasicRepresentation(string format);

		ITypeMetadata GetPartByTypeName(string partName);
		IPartCollection GetPartCollectionByDescriptiveName(string descriptiveName);
		IPartCollection GetPartCollectionContainingPartName(string partName);
		IPartCollection GetPartCollectionContainingType(Type partType);
	}
}