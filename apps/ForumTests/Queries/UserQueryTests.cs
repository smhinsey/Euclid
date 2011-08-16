using Euclid.TestingSupport;
using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NHibernate;
using NUnit.Framework;


namespace ForumTests.Queries
{
	public class UserQueryTests : NhTestFixture<User>
	{
		public UserQueryTests() : 
			base(new AutoMapperConfiguration(typeof(User)))
		{
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
	}
}