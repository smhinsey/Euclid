using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Castle.Windsor;
using Euclid.Agent.Extensions;
using Euclid.Composites;
using Euclid.Composites.AgentResolution;
using Euclid.Framework.Agent.Metadata;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
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

			composite.AddAgent(typeof (FakeCommand).Assembly);

			composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("CompositeTestsDb"), true, false);

			composite.Configure(new CompositeAppSettings());

			var query = container.Resolve<FakeQuery>();

			Assert.NotNull(query);
		}
	}
}