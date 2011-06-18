using Castle.Windsor;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeDispatcher : MultitaskingMessageDispatcher<FakeRegistry>
	{
		public FakeDispatcher(IWindsorContainer container, FakeRegistry registry) : base(container, registry)
		{
		}
	}
}