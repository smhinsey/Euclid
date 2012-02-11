using System;
using System.Linq;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	[StepScope(Scenario = "Vote Comment Up")]
	[StepScope(Scenario = "Vote Comment Down")]
	public class VoteOnCommentScenarios : PublishPostSpecification,
	                                      ICommandPublishStep<VoteOnComment>,
	                                      ICommandPublishStep<CommentOnPost>,
	                                      ICommandCompleteStep<CommentOnPost>
	{
		private const string CommentIdentifierKey = "CommentIdentifier";

		private readonly Guid _userForumIdentifier = Guid.Empty;

		private Guid CommentIdentifier
		{
			get
			{
				return (Guid)ScenarioContext.Current[CommentIdentifierKey];
			}
			set
			{
				ScenarioContext.Current[CommentIdentifierKey] = value;
			}
		}

		public void CommandCompleted(IPublicationRecord record, CommentOnPost command)
		{
			var query = Container.Resolve<CommentQueries>();

			var postDetails = query.FindCommentsBelongingToPost(_userForumIdentifier, PostIdentifier);

			var comment = postDetails.Comments.Where(c => c.Title == command.Title).FirstOrDefault();

			Assert.NotNull(comment);

			CommentIdentifier = comment.Identifier;
		}

		public VoteOnComment GetCommand(VoteOnComment command)
		{
			command.CommentIdentifier = CommentIdentifier;

			return command;
		}

		public CommentOnPost GetCommand(CommentOnPost command)
		{
			command.PostIdentifier = PostIdentifier;

			return command;
		}

		[Then(@"the Comment has a score of (.*)")]
		public void VerifyCommentScore(int expectedScore)
		{
			var query = Container.Resolve<CommentQueries>();

			Assert.NotNull(query);

			var comment = query.FindById(CommentIdentifier);

			Assert.NotNull(comment);

			Assert.AreEqual(expectedScore, comment.Score);
		}
	}
}