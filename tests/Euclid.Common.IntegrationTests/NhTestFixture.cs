using Euclid.Common.TestingFakes.Storage.Model;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests
{
	public class NhTestFixture
	{
		protected ISessionFactory SessionFactory;

		[SetUp]
		public void SetUp()
		{
			var cfg = new AutoMapperConfiguration();

			SessionFactory = Fluently
				.Configure()
				.Database(SQLiteConfiguration.Standard.UsingFile("NhSimpleRepositoryTests"))
				.Mappings(map => map.AutoMappings.Add(AutoMap.AssemblyOf<FakeModel>(cfg)))
				.ExposeConfiguration(buildSchema)
				.BuildSessionFactory();
		}

		private static void buildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}