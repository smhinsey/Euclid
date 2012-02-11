using System;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	public class PublishPostSpecification : ForumSpecifications, ICommandCompleteStep<PublishPost>
	{
		private const string PostIdentifierKey = "PostIdentifier";

		private readonly Guid _userForumIdentifier = Guid.Empty;

		protected Guid PostIdentifier
		{
			get
			{
				return (Guid)ScenarioContext.Current[PostIdentifierKey];
			}
			set
			{
				ScenarioContext.Current[PostIdentifierKey] = value;
			}
		}

		protected PostQueries PostQueries
		{
			get
			{
				return Container.Resolve<PostQueries>();
			}
		}

		public void CommandCompleted(IPublicationRecord record, PublishPost command)
		{
			var post = PostQueries.FindByTitle(_userForumIdentifier, command.Title);

			Assert.NotNull(post);

			PostIdentifier = post.Identifier;
		}
	}
}