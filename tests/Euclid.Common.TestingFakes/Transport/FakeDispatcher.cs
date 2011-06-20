using Euclid.Common.Messaging;
using Euclid.Common.TestingFakes.Registry;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeDispatcher : MultitaskingMessageDispatcher<FakeRegistry>
	{
		public FakeDispatcher(IServiceLocator container, FakeRegistry publicationRegistry) : base(container, publicationRegistry)
		{
		}
	}
}