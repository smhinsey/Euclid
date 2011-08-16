using System.Threading;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;

namespace ForumTests.EndToEnd
{
	public class ForumEndToEndTests : HostingFabricFixture
	{
		private const int SleepForCommand = 1500;

		public ForumEndToEndTests() :
			base(typeof(PublishPost).Assembly)
		{
		}

		[Test]
		public void TestCommentOnPost()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			const string commentTitle = "Comment Title";
			const string commentBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody});

			Thread.Sleep(SleepForCommand);

			var query = Container.Resolve<PostQueries>();

			var post = query.FindByTitle(postTitle);

			publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = commentTitle, Body = commentBody});

			Thread.Sleep(SleepForCommand);

			var anotherQuery = Container.Resolve<CommentQueries>();

			var comments = anotherQuery.FindCommentsBelongingToPost(post.Identifier);

			Assert.AreEqual(1, comments.Count);
		}

		[Test]
		public void TestPublishPost()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody});

			Thread.Sleep(SleepForCommand);

			var query = Container.Resolve<PostQueries>();

			var post = query.FindByTitle(postTitle);

			Assert.IsNotNull(post);
			Assert.AreEqual(postTitle, post.Title);
			Assert.AreEqual(postBody, post.Body);
		}

		[Test]
		public void TestVoteOnComment()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			const string commentTitle = "Comment Title";
			const string commentBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody});

			Thread.Sleep(SleepForCommand);

			var query = Container.Resolve<PostQueries>();

			var post = query.FindByTitle(postTitle);

			publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = commentTitle, Body = commentBody});

			Thread.Sleep(SleepForCommand);

			var anotherQuery = Container.Resolve<CommentQueries>();

			var comments = anotherQuery.FindCommentsBelongingToPost(post.Identifier);

			publisher.PublishMessage(new VoteOnComment {CommentIdentifier = comments[0].Identifier, VoteUp = true});

			Thread.Sleep(SleepForCommand);

			var yetAnotherQuery = Container.Resolve<CommentQueries>();

			var comment = yetAnotherQuery.FindById(comments[0].Identifier);

			Assert.AreEqual(1, comment.Score);
		}

		[Test]
		public void TestVoteOnPost()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();
			var query = Container.Resolve<PostQueries>();

			publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody});

			Thread.Sleep(SleepForCommand);

			var post = query.FindByTitle(postTitle);

			Assert.AreEqual(0, post.Score);

			publisher.PublishMessage(new VoteOnPost {PostIdentifier = post.Identifier, VoteUp = true});

			Thread.Sleep(SleepForCommand);

			var anotherQuery = Container.Resolve<PostQueries>();

			var postCopy = anotherQuery.FindByTitle(postTitle);

			Assert.AreEqual(1, postCopy.Score);
		}
	}
}