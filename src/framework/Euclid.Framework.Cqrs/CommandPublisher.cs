using Euclid.Common.Logging;
using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	public class CommandPublisher : DefaultPublisher, ILoggingSource
	{
		public CommandPublisher(ICommandRegistry registry, IMessageChannel channel)
			: base(registry, channel)
		{
		}
	}
}
