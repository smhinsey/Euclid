using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;

namespace ForumTests.EndToEnd
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class ForumEndToEndTests : HostingFabricFixture
	{
		public ForumEndToEndTests() :
			base(typeof (PublishPost).Assembly)
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

			WaitUntilComplete(
			               publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody}));

			var query = Container.Resolve<PostQueries>();

			var post = query.FindByTitle(postTitle);

			WaitUntilComplete(
			               publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = commentTitle, Body = commentBody}));

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

			WaitUntilComplete(
			               publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody}));

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

			WaitUntilComplete(
			               publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody}));

			var query = Container.Resolve<PostQueries>();

			var post = query.FindByTitle(postTitle);

			WaitUntilComplete(
			               publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = commentTitle, Body = commentBody}));

			var anotherQuery = Container.Resolve<CommentQueries>();

			var comments = anotherQuery.FindCommentsBelongingToPost(post.Identifier);

			WaitUntilComplete(
			               publisher.PublishMessage(new VoteOnComment {CommentIdentifier = comments[0].Identifier, VoteUp = true}));

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

			WaitUntilComplete(
			               publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody}));

			var post = query.FindByTitle(postTitle);

			Assert.AreEqual(0, post.Score);

			WaitUntilComplete(
			               publisher.PublishMessage(new VoteOnPost {PostIdentifier = post.Identifier, VoteUp = true}));

			var anotherQuery = Container.Resolve<PostQueries>();

			var postCopy = anotherQuery.FindByTitle(postTitle);

			Assert.AreEqual(1, postCopy.Score);
		}
	}
}