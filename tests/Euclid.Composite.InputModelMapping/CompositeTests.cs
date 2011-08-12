using Castle.Windsor;
using Euclid.Composites;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace Euclid.Composite.InputModelMapping
{
    [TestFixture]
    public class CompositeTests
    {

			[Test]
			public void TestResolveQuery()
			{
				var container = new WindsorContainer();

				var composite = new BasicCompositeApp(container);

				composite.AddAgent(typeof(FakeCommand).Assembly);

				composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("CompositeTestsDb"), true, false);

				composite.Configure(new CompositeAppSettings());

				var query = container.Resolve<FakeQuery>();

				Assert.NotNull(query);
			}
    }
}