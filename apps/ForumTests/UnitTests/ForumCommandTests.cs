using System.Threading;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using NUnit.Framework;

namespace ForumTests.UnitTests
{
	public class ForumCommandTests : HostingFabricFixture
	{
		[Test]
		public void TestCommentOnPost()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void TestPublishPost()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();
			var query = Container.Resolve<PostQueries>();

			publisher.PublishMessage(new PublishPost {Title = postTitle, Body = postBody});

			Thread.Sleep(5000);

			var post = query.FindByTitle(postTitle);

			Assert.IsNotNull(post);
			Assert.AreEqual(postTitle, post.Title);
			Assert.AreEqual(postBody, post.Body);
		}

		[Test]
		public void TestVoteOnComment()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void TestVoteOnPost()
		{
			const string postTitle = "Post Title";
			const string postBody = "Lorem ipsum dolor sit amet consecutator.";

			var publisher = Container.Resolve<IPublisher>();
			var query = Container.Resolve<PostQueries>();

			publisher.PublishMessage(new PublishPost { Title = postTitle, Body = postBody });

			Thread.Sleep(5000);

			var post = query.FindByTitle(postTitle);

			Assert.AreEqual(0, post.Score);

			publisher.PublishMessage(new VoteOnPost() { PostIdentifier = post.Identifier, VoteUp = true });

			Thread.Sleep(5000);
			var anotherQuery = Container.Resolve<PostQueries>();

			var postCopy = anotherQuery.FindByTitle(postTitle);

			Assert.AreEqual(1, postCopy.Score);
		}
	}
}