using System;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	public class PublishPostSpecification : ForumSpecifications, ICommandCompleteStep<PublishPost>
	{
		private const string PostIdentifierKey = "PostIdentifier";

		public void CommandCompleted(IPublicationRecord record, PublishPost command)
		{
			var post = PostQueries.FindByTitle(command.Title);

			Assert.NotNull(post);

			PostIdentifier = post.Identifier;
		}

		protected Guid PostIdentifier
		{
			get { return (Guid) ScenarioContext.Current[PostIdentifierKey]; }
			set { ScenarioContext.Current[PostIdentifierKey] = value; }
		}

		protected PostQueries PostQueries
		{
			get { return Container.Resolve<PostQueries>(); }
		}
	}
}