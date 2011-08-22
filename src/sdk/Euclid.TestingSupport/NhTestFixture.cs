using Euclid.Framework.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Euclid.TestingSupport
{
	public class NhTestFixture<TEntity>
	{
		protected ISessionFactory SessionFactory;

		private readonly AutoMapperConfiguration _config;

		public NhTestFixture(AutoMapperConfiguration config)
		{
			this._config = config;
		}

		[SetUp]
		public void SetUp()
		{
			this.SessionFactory =
				Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("NhTestFixtureDb")).Mappings(
					map => map.AutoMappings.Add(AutoMap.AssemblyOf<TEntity>(this._config).IgnoreBase<DefaultReadModel>)).
					ExposeConfiguration(buildSchema).BuildSessionFactory();
		}

		private static void buildSchema(Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}