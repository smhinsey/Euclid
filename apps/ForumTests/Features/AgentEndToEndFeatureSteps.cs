using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using AgentConsole;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.HostingFabric;
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using ForumAgent.Queries;
using Microsoft.WindowsAzure;
using NUnit.Framework;
using TechTalk.SpecFlow;
using log4net.Config;

namespace ForumTests.Features
{
	[Binding]
	public class AgentEndToEndFeatureSteps
	{
		private const string CommentBody = "Lorem ipsum dolor sit amet consecutator.";
		private const string CommentTitle = "Comment Title";
		private const string PostBody = "Lorem ipsum dolor sit amet consecutator.";
		private const string PostTitle = "Post Title";
		
		private readonly Guid _categoryId = Guid.NewGuid();

		private bool _configured;
		private WindsorContainer _container;
		private ConsoleFabric _fabric;
		private Guid _pubIdOfLastMessage;

		[Given(@"a runtime fabric for agent ForumAgent")]
		public void GivenARuntimeFabricForAgent()
		{
			if (!_configured)
			{
				configure(typeof (PublishPost).Assembly);
			}
		}

		[Given(@"I publish the command PublishPost")]
		public void GivenIPublishThecommandPublishPost()
		{
			var publisher = _container.Resolve<IPublisher>();

			_pubIdOfLastMessage = publisher.PublishMessage(new PublishPost { Title = PostTitle, Body = PostBody, CategoryIdentifier = _categoryId });
		}

		[Then(@"the query CommentQueries returns the Comment")]
		public void ThenTheQueryCommentQueriesReturnsTheComment()
		{
			var postQueries = _container.Resolve<PostQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var commentQueries = _container.Resolve<CommentQueries>();

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			Assert.AreEqual(1, comments.Count);
		}


		[Then(@"the query ForumQueries returns the Post")]
		public void ThenTheQueryForumQueriesReturnsThePost()
		{
			var query = _container.Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.IsNotNull(post);
			Assert.AreEqual(PostTitle, post.Title);
			Assert.AreEqual(PostBody, post.Body);
		}

		[Then(@"the query CategoryQueries returns Post")]
		public void ThenTheQueryCategoryQueriesReturnsPost()
		{
			var postQueries = _container.Resolve<PostQueries>();

			var posts = postQueries.FindPostsByCategory(_categoryId);

			Assert.IsNotNull(posts);
			Assert.AreEqual(PostTitle, posts[0].Title);
			Assert.AreEqual(PostBody, posts[0].Body);
			Assert.AreEqual(_categoryId, posts[0].CategoryIdentifier);
		}

		[Then(@"the query ForumQueries returns the Score")]
		public void ThenTheQueryForumQueriesReturnsTheScore()
		{
			var query = _container.Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			Assert.AreEqual(1, post.Score);
		}

		[Then(@"the query CommentQueries returns the Score")]
		public void ThenTheQueryCommentQueriesReturnsTheScore()
		{
			var postQueries = _container.Resolve<PostQueries>();
			var commentQueries = _container.Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			var comment = commentQueries.FindById(comments[0].Identifier);

			Assert.AreEqual(1, comment.Score);
		}

		[When(@"I publish the command CommentOnPost")]
		public void WhenIPublishThecommandCommentOnPost()
		{
			var publisher = _container.Resolve<IPublisher>();

			var query = _container.Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			_pubIdOfLastMessage = publisher.PublishMessage(new CommentOnPost {PostIdentifier = post.Identifier, Title = CommentTitle, Body = CommentBody});
		}

		[When(@"I publish the command VoteOnComment")]
		public void WhenIPublishThecommandVoteOnComment()
		{
			var publisher = _container.Resolve<IPublisher>();

			var postQueries = _container.Resolve<PostQueries>();
			var commentQueries = _container.Resolve<CommentQueries>();

			var post = postQueries.FindByTitle(PostTitle);

			var comments = commentQueries.FindCommentsBelongingToPost(post.Identifier);

			_pubIdOfLastMessage = publisher.PublishMessage(new VoteOnComment {CommentIdentifier = comments[0].Identifier, VoteUp = true});
		}

		[When(@"I publish the command VoteOnPost")]
		public void WhenIPublishThecommandVoteOnPost()
		{
			var publisher = _container.Resolve<IPublisher>();

			var query = _container.Resolve<PostQueries>();

			var post = query.FindByTitle(PostTitle);

			_pubIdOfLastMessage = publisher.PublishMessage(new VoteOnPost { PostIdentifier = post.Identifier, VoteUp = true });
		}

		[When(@"the command is complete")]
		public void WhenTheCommandIsComplete()
		{
			while (true)
			{
				var registry = _container.Resolve<ICommandRegistry>();

				var record = registry.GetRecord(_pubIdOfLastMessage);

				if (record.Completed || record.Error)
				{
					break;
				}

				Thread.Sleep(250);
			}
		}

		private void configure(Assembly agentAssembly)
		{
			XmlConfigurator.Configure();

			_container = new WindsorContainer();

			setAzureCredentials(_container);

			_fabric = new ConsoleFabric(_container);

			var composite = new BasicCompositeApp(_container);

			composite.RegisterNh(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), true, false);

			composite.AddAgent(agentAssembly);

			composite.Configure(getCompositeSettings());

			_fabric.Initialize(getFabricSettings());

			_fabric.InstallComposite(composite);

			_fabric.Start();

			_configured = true;
		}


		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.MessageChannel.WithDefault(typeof (AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private void setAzureCredentials(IWindsorContainer container)
		{
			// as soon as we can stop using the azure storage emulator we should

			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}