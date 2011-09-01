using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.TestingSupport;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace Euclid.Composite.InputModelMapping
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class CompositeTests
	{
		[Test]
		public void TestResolveQuery()
		{
			var container = new WindsorContainer();

			var composite = new BasicCompositeApp(container)
				{ Name = "Euclid.Composite.InputModelMapping.CompositeTests", Description = "A composite used for testing" };

			composite.AddAgent(typeof(TestCommand).Assembly);

			composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("CompositeTestsDb"), false);

			var settings = new CompositeAppSettings();

			settings.OutputChannel.ApplyOverride(typeof(InMemoryMessageChannel));

			composite.Configure(settings);

			var query = container.Resolve<TestQuery>();

			Assert.NotNull(query);
		}
	}
}