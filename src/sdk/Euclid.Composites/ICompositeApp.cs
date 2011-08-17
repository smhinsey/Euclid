using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IList<IAgentMetadata> Agents { get; }
		CompositeApplicationState State { get; set; }
		void AddAgent(Assembly assembly);
		void Configure(CompositeAppSettings compositeAppSettings);
		void RegisterInputModel(IInputToCommandConverter converter);
	}
}