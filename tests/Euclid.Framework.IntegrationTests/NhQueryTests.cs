using System;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Framework.TestingFakes.Cqrs;
using Euclid.TestingSupport;
using NUnit.Framework;
using AutoMapperConfiguration = Euclid.TestingSupport.AutoMapperConfiguration;

namespace Euclid.Framework.IntegrationTests
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class NhQueryTests : NhTestFixture<FakeReadModel>
	{
		private const string ModelMessage = "Lorem ipsum";

		public NhQueryTests() : base(new AutoMapperConfiguration(typeof (FakeReadModel)))
		{
		}

		private Guid createFakeData()
		{
			var model = new FakeReadModel
			            	{
			            		Created = DateTime.Today,
			            		Modified = DateTime.Today,
			            		Message = ModelMessage
			            	};

			using (var session = SessionFactory.OpenSession())
			{
				session.Save(model);

				session.Flush();
			}

			return model.Identifier;
		}

		[Test]
		public void FindByCreated()
		{
			createFakeData();

			var query = new NhQuery<FakeReadModel>(SessionFactory.OpenSession());

			var result = query.FindByCreationDate(DateTime.Today);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(ModelMessage, result[0].Message);
		}

		[Test]
		public void FindById()
		{
			var id = createFakeData();

			var query = new NhQuery<FakeReadModel>(SessionFactory.OpenSession());

			var result = query.FindById(id);

			Assert.IsNotNull(result);
			Assert.AreEqual(ModelMessage, result.Message);
		}

		[Test]
		public void FindByModified()
		{
			createFakeData();

			var query = new NhQuery<FakeReadModel>(SessionFactory.OpenSession());

			var result = query.FindByModificationDate(DateTime.Today);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(ModelMessage, result[0].Message);
		}
	}
}