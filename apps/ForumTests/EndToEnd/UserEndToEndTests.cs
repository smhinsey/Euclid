using System.Threading;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;

namespace ForumTests.EndToEnd
{
	public class UserEndToEndTests : HostingFabricFixture
	{
		private const int SleepForCommand = 1500;

		public UserEndToEndTests() :
			base(typeof(RegisterUser).Assembly)
		{
		}

		[Test]
		public void TestRegisterUser()
		{
			const string username = "johndoe";
			var passwordHash = username.GetHashCode().ToString();
			var passwordSalt = username.GetHashCode().ToString();

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new RegisterUser {Username = username, PasswordHash = passwordHash, PasswordSalt = passwordSalt});

			Thread.Sleep(SleepForCommand);

			var query = Container.Resolve<UserQueries>();

			var user = query.FindByUsername(username);

			Assert.IsNotNull(user);
			Assert.AreEqual(passwordHash, user.PasswordHash);
			Assert.AreEqual(passwordSalt, user.PasswordSalt);
		}

		[Test]
		public void TestUpdateUserProfile()
		{
			const string username = "johndoe";
			const string email = "johndoe@email.service";
			var passwordHash = username.GetHashCode().ToString();
			var passwordSalt = username.GetHashCode().ToString();

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new RegisterUser {Username = username, PasswordHash = passwordHash, PasswordSalt = passwordSalt});

			Thread.Sleep(SleepForCommand);

			var query = Container.Resolve<UserQueries>();

			var user = query.FindByUsername(username);

			publisher.PublishMessage(new UpdateUserProfile {Email = email, UserIdentifier = user.Identifier});

			Thread.Sleep(SleepForCommand);

			var anotherQuery = Container.Resolve<UserQueries>();

			var profile = anotherQuery.FindByUserIdentifier(user.Identifier);

			Assert.IsNotNull(profile);
			Assert.AreEqual(email, profile.Email);
		}
	}
}