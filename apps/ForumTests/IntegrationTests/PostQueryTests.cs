using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NHibernate;
using NUnit.Framework;

namespace ForumTests.IntegrationTests
{
	public class PostQueryTests : NhTestFixture
	{
		[Test]
		public void TestListPostsByCategoryName()
		{
			var session = SessionFactory.OpenSession();

			addFakeData(session);

			var query = new PostQueries(session);

			var results = query.FindPostsByCategory("all");

			Assert.IsNotNull(results);
		}

		private static void addFakeData(ISession session)
		{
			var category = new Category()
			               	{
			               		CommentCount = 0,
			               		PostCount = 0,
			               		Name = "all"
			               	};

			session.Save(category);

			var post = new Post()
			           	{
			           		Body = "Lorem ipsum",
			           		CategoryIdentifier = category.Identifier,
			           		Title = "Latin is great!"
			           	};

			session.Save(post);

			session.Flush();
		}
	}
}