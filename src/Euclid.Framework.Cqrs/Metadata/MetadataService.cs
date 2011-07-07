using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Framework.Cqrs.Metadata
{
	public class MetadataService : IExtractor
	{
		private readonly IList<Type> _commandTypes;

		public MetadataService(IExtractorSettings settings)
		{
			foreach (var assembly in settings.AssembliesContainingCommands.Value)
			{
				_commandTypes = assembly.GetTypes().Where(x => x.GetInterface("ICommand") != null).ToList();
			}
		}

		public ICommand CreateCommand(Type commandType)
		{
			return Activator.CreateInstance(commandType) as ICommand;
		}

		public IList<Type> GetVisibleCommandTypes()
		{
			return _commandTypes;
		}
	}
}