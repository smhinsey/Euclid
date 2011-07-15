using Euclid.Common.TestingFakes.Storage;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests.Storage.Model
{
	public class NhSimpleRepositoryTests
	{
		private ISessionFactory _sessionFactory;

		[SetUp]
		public void SetUp()
		{
			var cfg = new MessageConfiguration();

			_sessionFactory = Fluently
				.Configure()
				.Database(SQLiteConfiguration.Standard.InMemory())
				.Mappings(map => map.AutoMappings.Add(AutoMap.AssemblyOf<FakeMessage>(cfg)))
				.ExposeConfiguration(buildSchema)
				.BuildSessionFactory();
		}

		[Test]
		public void Test()
		{
		}

		private static void buildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}