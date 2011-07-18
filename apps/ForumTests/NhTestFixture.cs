using Euclid.Framework;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ForumAgent.ReadModels;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace ForumTests
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
				.Database(SQLiteConfiguration.Standard.UsingFile("NhTestFixtureDb"))
				.Mappings(map => map.AutoMappings.Add(AutoMap.AssemblyOf<Category>(cfg).IgnoreBase<DefaultReadModel>))
				.ExposeConfiguration(buildSchema)
				.BuildSessionFactory();
		}

		private static void buildSchema(Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}