using System;
using System.Diagnostics;
using Euclid.Common.Storage.NHibernate;
using Euclid.Common.TestingFakes.Storage;
using Euclid.Common.TestingFakes.Storage.Model;
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
			var cfg = new AutoMapperConfiguration();

			_sessionFactory = Fluently
				.Configure()
				.Database(SQLiteConfiguration.Standard.UsingFile("NhSimpleRepositoryTests"))
				.Mappings(map => map.AutoMappings.Add(AutoMap.AssemblyOf<FakeModel>(cfg)))
				.ExposeConfiguration(buildSchema)
				.BuildSessionFactory();
		}

		[Test]
		public void Save()
		{
			var model = new FakeModel()
			            	{
			            		Created = DateTime.Now,
			            		Modified = DateTime.Now,
			            		Name = "Name"
			            	};

			var repo = new NhSimpleRepository<FakeModel>(_sessionFactory.OpenSession());

			repo.Save(model);
		}

		[Test]
		public void Delete()
		{
			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = DateTime.Now,
				Name = "Name"
			};

			var repo = new NhSimpleRepository<FakeModel>(_sessionFactory.OpenSession());

			var saved = repo.Save(model);

			repo.Delete(saved);

			var deleted = repo.FindById(saved.Identifier);

			Assert.IsNull(deleted);
		}

		[Test]
		public void DeleteById()
		{
			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = DateTime.Now,
				Name = "Name"
			};

			var repo = new NhSimpleRepository<FakeModel>(_sessionFactory.OpenSession());

			var saved = repo.Save(model);

			repo.Delete(saved.Identifier);

			var deleted = repo.FindById(saved.Identifier);

			Assert.IsNull(deleted);
		}

		[Test]
		public void Update()
		{
			const string firstName = "Name1";
			const string secondName = "Name2";

			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = DateTime.Now,
				Name = firstName
			};

			var repo = new NhSimpleRepository<FakeModel>(_sessionFactory.OpenSession());

			var saved = repo.Save(model);

			saved.Name = secondName;

			var updated = repo.Update(saved);

			Assert.AreEqual(secondName, updated.Name);
		}

		[Test]
		public void FindById()
		{
			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = DateTime.Now,
				Name = "Name"
			};

			var session = _sessionFactory.OpenSession();

			var repo = new NhSimpleRepository<FakeModel>(session);

			var original = repo.Save(model);

			var result = repo.FindById(original.Identifier);
			
			Assert.IsNotNull(result);
			Assert.AreEqual(model.Name, result.Name);
		}

		[Test]
		public void FindByDateCreated()
		{
			var now = DateTime.Now;

			var model = new FakeModel()
			{
				Created = now,
				Modified = DateTime.Now,
				Name = "Name"
			};

			var session = _sessionFactory.OpenSession();

			var repo = new NhSimpleRepository<FakeModel>(session);

			repo.Save(model);

			var result = repo.FindByCreationDate(now);

			Assert.IsNotNull(result);
			Assert.AreEqual(model.Name, result[0].Name);
		}

		[Test]
		public void FindByDateCreatedRange()
		{
			var now = DateTime.Now;

			var model = new FakeModel()
			{
				Created = now,
				Modified = DateTime.Now,
				Name = "Name"
			};

			var session = _sessionFactory.OpenSession();

			var repo = new NhSimpleRepository<FakeModel>(session);

			repo.Save(model);

			var result = repo.FindByCreationDate(now.AddDays(-1), now.AddDays(1));

			Assert.IsNotNull(result);
			Assert.AreEqual(model.Name, result[0].Name);
		}

		[Test]
		public void FindByDateModified()
		{
			var now = DateTime.Now;

			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = now,
				Name = "Name"
			};

			var session = _sessionFactory.OpenSession();

			var repo = new NhSimpleRepository<FakeModel>(session);

			repo.Save(model);

			var result = repo.FindByModificationDate(now);

			Assert.IsNotNull(result);
			Assert.AreEqual(model.Name, result[0].Name);
		}

		[Test]
		public void FindByDateModifiedRange()
		{
			var now = DateTime.Now;

			var model = new FakeModel()
			{
				Created = DateTime.Now,
				Modified = now,
				Name = "Name"
			};

			var session = _sessionFactory.OpenSession();

			var repo = new NhSimpleRepository<FakeModel>(session);

			repo.Save(model);

			var result = repo.FindByModificationDate(now.AddDays(-1), now.AddDays(1));

			Assert.IsNotNull(result);
			Assert.AreEqual(model.Name, result[0].Name);
		}

		private static void buildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}