using Euclid.Common.ServiceHost;

namespace Euclid.Framework.EventSourcing
{
	public class EventHost : DefaultHostedService
	{
		protected override void OnStart()
		{
			// set up EventStore and wire it into the container

			throw new System.NotImplementedException();
		}

		protected override void OnStop()
		{
			// take a snapshot and shut down

			throw new System.NotImplementedException();
		}
	}
}