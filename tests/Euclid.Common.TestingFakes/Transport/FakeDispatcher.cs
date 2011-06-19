using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeDispatcher : MultitaskingMessageDispatcher<FakeRegistry>
	{
		public FakeDispatcher(IServiceLocator container, FakeRegistry registry) : base(container, registry)
		{
		}
	}
}