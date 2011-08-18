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
	public class ForumSteps : DefaultSpecSteps
	{
		private const string CommentBody = "Lorem ipsum dolor sit amet consecutator.";
		private const string CommentTitle = "Comment Title";
		private const string PostBody = "Lorem ipsum dolor sit amet consecutator.";
		private const string PostTitle = "Post Title";

		private readonly Guid _categoryId = Guid.NewGuid();

		[Then(@"the query CategoryQueries returns Post")]
		public void ThenTheQueryCategoryQueriesReturnsPost()
		{
			var postQueries = GetContainer().Resolve<PostQueries>();

			var posts = postQueries.FindPostsByCategory(_categoryId);

			Assert.IsNotNull(posts);
			Assert.AreEqual(PostTitle, posts[0].Title);
			Assert.AreEqual(PostBody, posts[0].Body);
			Assert.AreEqual(_categoryId, posts[0].CategoryIdentifier);
		}

		[Then(@"the query CommentQueries returns the Comment")]
		public void ThenTheQueryCommentQueriesReturnsTheComment()
		{
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			Assert.AreEqual(1, comments.Count);
		}

		[Then(@"the query CommentQueries returns the post with a score of (.*)")]
		public void ThenTheQueryCommentQueriesReturnsTheScore(int score)
		{
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			var comment = commentQueries.FindById(comments[0].Identifier);

			Assert.AreEqual(score, comment.Score);
		}


		[Then(@"the query ForumQueries returns the Post")]
		public void ThenTheQueryForumQueriesReturnsThePost()
		{
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.IsNotNull(post);
			Assert.AreEqual(PostTitle, post.Title);
			Assert.AreEqual(PostBody, post.Body);
		}

		[Then(@"the query ForumQueries returns the post with a score of (.*)")]
		public void ThenTheQueryForumQueriesReturnsTheScore(int score)
		{
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.AreEqual(score, post.Score);
		}

		[When(@"I publish the command PublishPost")]
		public void WhenIPublishTheCommandPublishPost()
		{
			var publisher = GetContainer().Resolve<IPublisher>();

			PubIdOfLastMessage = publisher.PublishMessage(new PublishPost {Title = PostTitle, Body = PostBody, CategoryIdentifier = _categoryId});
		}

		[When(@"I publish the command CommentOnPost")]
		public void WhenIPublishTheCommandCommentOnPost()
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			PubIdOfLastMessage = publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = CommentTitle, Body = CommentBody});
		}

		[When(@"I publish the command VoteOnComment VoteUp=(.*)")]
		public void WhenIPublishTheCommandVoteOnComment(bool direction)
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			PubIdOfLastMessage = publisher.PublishMessage(new VoteOnComment {CommentIdentifier = comments[0].Identifier, VoteUp = direction});
		}

		[When(@"I publish the command VoteOnPost VoteUp=(.*)")]
		public void WhenIPublishTheCommandVoteOnPost(bool direction)
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			PubIdOfLastMessage = publisher.PublishMessage(new VoteOnPost {PostIdentifier = post.Identifier, VoteUp = direction});
		}
	}
}