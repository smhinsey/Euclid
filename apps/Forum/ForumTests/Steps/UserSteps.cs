using System;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	[StepScope(Feature = "User Profiles")]
	public class UserSteps : ForumSpecifications,
	                         ICommandCompleteStep<RegisterForumUser>,
	                         ICommandPublishStep<UpdateUserProfile>
	{
		private const string UserIdentifierKey = "UserIdentifier";

		private readonly Guid _userForumIdentifier = Guid.Empty;

		private Guid UserIdentifier
		{
			get
			{
				return (Guid)ScenarioContext.Current[UserIdentifierKey];
			}
			set
			{
				ScenarioContext.Current[UserIdentifierKey] = value;
			}
		}

		[Then(@"running Authenticate on  UserQueries will return true")]
		public void AuthenticateUser()
		{
			var query = Container.Resolve<UserQueries>();

			Assert.True(query.Authenticate(_userForumIdentifier, "jimmyjon", "hash"));
		}

		public void CommandCompleted(IPublicationRecord record, RegisterForumUser command)
		{
			var query = Container.Resolve<UserQueries>();

			var user = query.FindByUsername(_userForumIdentifier, "johndoe");

			Assert.NotNull(user);

			UserIdentifier = user.Identifier;
		}

		public UpdateUserProfile GetCommand(UpdateUserProfile command)
		{
			command.UserIdentifier = UserIdentifier;

			return command;
		}
	}
}