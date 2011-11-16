using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Common.Messaging;
using Euclid.Framework.Models;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	[StepScope(Feature = "Forum Posting")]
	public class PostingSpecification : ForumSpecifications,
	                                    ICommandCompleteStep<PublishPost>,
	                                    ICommandPublishStep<CommentOnPost>,
	                                    IValidateListOfReadModels<PostQueries, Post>
	{
		private readonly Guid _userForumIdentifier = Guid.Empty;

		private Post PublishedPost { get; set; }

		[Then(@"the resulting list contains a (.*) with values:")]
		public void CheckListForValue(string readModelPartTypeName, Table table)
		{
			var readModelType = AgentMetadata.GetPartByTypeName(readModelPartTypeName).Type;

			Assert.NotNull(readModelType);

			var readModelInstance = GetInstanceFromTable<IReadModel>(readModelType, table);

			Assert.NotNull(readModelInstance);

			var readModels = ScenarioContext.Current["Results"] as IList<Post>;

			Assert.NotNull(readModels);

			Assert.True(
				readModels.Any(
					r =>
					r.Title == "Post Title" && r.Body == "Post Body"
					&& r.CategoryIdentifier == new Guid("11111111-1111-1111-1111-111111111111")
					&& r.AuthorIdentifier == new Guid("00000000-0000-0000-0000-000000000000")
					&& r.ForumIdentifier == new Guid("33333333-3333-3333-3333-333333333333")));
		}

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

		public void ValidateList(PostQueries query, IList<Post> readModels)
		{
			Assert.True(readModels.Any(post => post.CategoryIdentifier == new Guid("11111111-1111-1111-1111-111111111111")));
				// ID is from .feature file

			ScenarioContext.Current["Results"] = readModels;
		}
	}
}