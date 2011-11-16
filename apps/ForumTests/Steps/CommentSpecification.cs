using System;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	[StepScope(Feature = "Forum Commenting")]
	public class CommentSpecification : ForumSpecifications,
	                                    ICommandCompleteStep<PublishPost>,
	                                    ICommandPublishStep<CommentOnPost>
	{
		private readonly Guid _userForumIdentifier = Guid.Empty;

		public CommentSpecification()
		{
			Initialize();
		}

		private Post PublishedPost { get; set; }

		public void CommandCompleted(IPublicationRecord record, PublishPost command)
		{
			Assert.IsTrue(record.Completed);

			Assert.IsFalse(record.Error, record.ErrorMessage);

			var postQueries = Container.Resolve<PostQueries>();

			PublishedPost = postQueries.FindByTitle(_userForumIdentifier, "Post Title");
		}

		public CommentOnPost GetCommand(CommentOnPost command)
		{
			command.PostIdentifier = PublishedPost.Identifier;

			return command;
		}
	}
}