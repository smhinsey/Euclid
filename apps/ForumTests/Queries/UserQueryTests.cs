using Euclid.TestingSupport;
using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NHibernate;
using NUnit.Framework;

namespace ForumTests.Queries
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class UserQueryTests : NhTestFixture<User>
	{
		public UserQueryTests() :
			base(new AutoMapperConfiguration(typeof (User)))
		{
		}

		private static void createFakeUser(ISession session, string username, string password)
		{
			var user = new User
			           	{
			           		Username = username,
			           		PasswordHash = password,
			           		PasswordSalt = password
			           	};

			session.Save(user);

			session.Flush();
		}

		[Test]
		public void TestAuthenticationQuery()
		{
			var session = SessionFactory.OpenSession();

			const string username = "johndoe";
			const string password = "password!";

			createFakeUser(session, username, password);

			var userQueries = new UserQueries(session);

			Assert.IsTrue(userQueries.Authenticate(username, password));
		}
	}
}