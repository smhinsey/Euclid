using System;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	/// <summary>
	/// 	The forum steps.
	/// </summary>
	public class ForumSteps : DefaultSpecSteps
	{
		private const string CommentBody = "Lorem ipsum dolor sit amet consecutator.";

		private const string CommentTitle = "Comment Title";

		private const string PostBody = "Lorem ipsum dolor sit amet consecutator.";

		/// <summary>
		/// 	The post title.
		/// </summary>
		private const string PostTitle = "Post Title";

		/// <summary>
		/// 	The _category id.
		/// </summary>
		private readonly Guid categoryId = Guid.NewGuid();

		/// <summary>
		/// 	The then the query category queries returns post.
		/// </summary>
		[Then(@"the query CategoryQueries returns Post")]
		public void ThenTheQueryCategoryQueriesReturnsPost()
		{
			var postQueries = GetContainer().Resolve<PostQueries>();

			var posts = postQueries.FindPostsByCategory(categoryId);

			Assert.IsNotNull(posts);
			Assert.AreEqual(PostTitle, posts[0].Title);
			Assert.AreEqual(PostBody, posts[0].Body);
			Assert.AreEqual(categoryId, posts[0].CategoryIdentifier);
		}

		/// <summary>
		/// 	The then the query comment queries returns the comment.
		/// </summary>
		[Then(@"the query CommentQueries returns the Comment")]
		public void ThenTheQueryCommentQueriesReturnsTheComment()
		{
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var postDetail = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			Assert.AreEqual(1, postDetail.Comments.Count);
		}

		/// <summary>
		/// 	The then the query comment queries returns the score.
		/// </summary>
		/// <param name = "score">
		/// 	The score.
		/// </param>
		[Then(@"the query CommentQueries returns the post with a score of (.*)")]
		public void ThenTheQueryCommentQueriesReturnsTheScore(int score)
		{
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var postDetail = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			var comment = commentQueries.FindById(postDetail.Comments[0].Identifier);

			Assert.AreEqual(score, comment.Score);
		}

		/// <summary>
		/// 	The then the query forum queries returns the post.
		/// </summary>
		[Then(@"the query ForumQueries returns the Post")]
		public void ThenTheQueryForumQueriesReturnsThePost()
		{
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.IsNotNull(post);
			Assert.AreEqual(PostTitle, post.Title);
			Assert.AreEqual(PostBody, post.Body);
		}

		/// <summary>
		/// 	The then the query forum queries returns the score.
		/// </summary>
		/// <param name = "score">
		/// 	The score.
		/// </param>
		[Then(@"the query ForumQueries returns the post with a score of (.*)")]
		public void ThenTheQueryForumQueriesReturnsTheScore(int score)
		{
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.AreEqual(score, post.Score);
		}

		/// <summary>
		/// 	The when i publish the command comment on post.
		/// </summary>
		[When(@"I publish the command CommentOnPost")]
		public void WhenIPublishTheCommandCommentOnPost()
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			PubIdOfLastMessage =
				publisher.PublishMessage(
					new CommentOnPost { PostIdentifier = post.Identifier, Title = CommentTitle, Body = CommentBody });
		}

		/// <summary>
		/// 	The when i publish the command publish post.
		/// </summary>
		[When(@"I publish the command PublishPost")]
		public void WhenIPublishTheCommandPublishPost()
		{
			var publisher = GetContainer().Resolve<IPublisher>();

			PubIdOfLastMessage =
				publisher.PublishMessage(new PublishPost { Title = PostTitle, Body = PostBody, CategoryIdentifier = categoryId });
		}

		/// <summary>
		/// 	The when i publish the command vote on comment.
		/// </summary>
		/// <param name = "direction">
		/// 	The direction.
		/// </param>
		[When(@"I publish the command VoteOnComment VoteUp=(.*)")]
		public void WhenIPublishTheCommandVoteOnComment(bool direction)
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var postQueries = GetContainer().Resolve<PostQueries>();
			var commentQueries = GetContainer().Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var postDetail = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			PubIdOfLastMessage =
				publisher.PublishMessage(
					new VoteOnComment { CommentIdentifier = postDetail.Comments[0].Identifier, VoteUp = direction });
		}

		/// <summary>
		/// 	The when i publish the command vote on post.
		/// </summary>
		/// <param name = "direction">
		/// 	The direction.
		/// </param>
		[When(@"I publish the command VoteOnPost VoteUp=(.*)")]
		public void WhenIPublishTheCommandVoteOnPost(bool direction)
		{
			var publisher = GetContainer().Resolve<IPublisher>();
			var query = GetContainer().Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			PubIdOfLastMessage = publisher.PublishMessage(
				new VoteOnPost { PostIdentifier = post.Identifier, VoteUp = direction });
		}
	}
}