using System;
using System.Reflection;

namespace Euclid.Framework.AgentMetadata
{
	public enum FormatterType
	{
		Basic = 0,

		Full
	};

	public interface IAgentMetadata
	{
		Assembly AgentAssembly { get; }

		IPartCollection Commands { get; }

		string Description { get; }

		string DescriptiveName { get; }

		bool IsValid { get; }

		IPartCollection Queries { get; }

		IPartCollection ReadModels { get; }

		string SystemName { get; }

		IMetadataFormatter GetFormatter(FormatterType style);

		IPartMetadata GetPartByTypeName(string partName);

		IPartCollection GetPartCollectionByDescriptiveName(string descriptiveName);

		IPartCollection GetPartCollectionContainingPartName(string partName);

		IPartCollection GetPartCollectionContainingType(Type partType);
	}
}