using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IList<IAgentMetadataFormatter> Agents { get; }
		CompositeApplicationState State { get; set; }
		void AddAgent(Assembly assembly);
		void Configure(CompositeAppSettings compositeAppSettings);
		void RegisterInputModel(IInputToCommandConverter converter);
	}
}