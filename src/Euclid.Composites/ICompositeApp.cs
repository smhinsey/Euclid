using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.Metadata;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IList<IAgentMetadata> Agents { get; }
		CompositeApplicationState ApplicationState { get; set; }
		void Configure(CompositeAppSettings compositeAppSettings);
		void InstallAgent(Assembly assembly);
		void RegisterInputModel(IInputToCommandConverter converter);
	}
}