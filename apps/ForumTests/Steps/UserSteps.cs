using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	public class UserSteps : DefaultSpecSteps
	{
		private const string Email = "johndoe@email.service";
		private const string Username = "johndoe";
		private const string Password = "password";

		[Given(@"I publish the command RegisterUser")]
		public void GivenIPublishTheCommandRegisterUser()
		{
			var publisher = GetContainer().Resolve<IPublisher>();

			PubIdOfLastMessage = publisher.PublishMessage(new RegisterUser { Username = Username, PasswordHash = Password, PasswordSalt = Password });
		}

		[Then(@"the query UserQueries returns the Profile")]
		public void ThenTheQueryUserQueriesReturnsTheProfile()
		{
			var query = GetContainer().Resolve<UserQueries>();

			var user = query.FindByUsername(Username);

			Assert.IsNotNull(user);
			Assert.AreEqual(Password, user.PasswordHash);
			Assert.AreEqual(Password, user.PasswordSalt);
		}

		[Then(@"the query UserQueries can authenticate")]
		public void ThenTheQueryUserQueriesCanAuthenticate()
		{
			var query = GetContainer().Resolve<UserQueries>();

			var authenticationResult = query.Authenticate(Username, Password);

			Assert.IsTrue(authenticationResult);
		}

		[Then(@"the query UserQueries returns the updated Profile")]
		public void ThenTheQueryUserQueriesReturnsTheUpdatedProfile()
		{
			var userQueries = GetContainer().Resolve<UserQueries>();

			var user = userQueries.FindByUsername(Username);
			var profile = userQueries.FindByUserIdentifier(user.Identifier);

			Assert.IsNotNull(profile);
			Assert.AreEqual(Email, profile.Email);
		}

		[When(@"I publish the command UpdateUserProfile")]
		public void WhenIPublishTheCommandUpdateUserProfile()
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var query = GetContainer().Resolve<UserQueries>();

			var user = query.FindByUsername(Username);

			PubIdOfLastMessage = publisher.PublishMessage(new UpdateUserProfile { Email = Email, UserIdentifier = user.Identifier });
		}
	}
}