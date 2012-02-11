using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class MarkPostAsFavoriteProcessor : DefaultCommandProcessor<MarkPostAsFavorite>
	{
		private readonly ISimpleRepository<ForumUserFavorite> _favoriteRepository;
		private readonly ISimpleRepository<Post> _postRepository;

		public MarkPostAsFavoriteProcessor(ISimpleRepository<ForumUserFavorite> favoriteRepository, ISimpleRepository<Post> postRepository)
		{
			_favoriteRepository = favoriteRepository;
			_postRepository = postRepository;
		}

		public override void Process(MarkPostAsFavorite message)
		{
			var post = _postRepository.FindById(message.PostIdentifier);

			var favorite = new ForumUserFavorite() 
			{ 
				Created = DateTime.Now, 
				Modified = (DateTime)SqlDateTime.MinValue, 
				AssociatedPostIdentifier = message.PostIdentifier,
				IsPost = true,
				ForumIdentifier = message.ForumIdentifier,
				UserIdentifier = message.UserIdentifier,
				AssociatedPostTitle = post.Title,
				Body = post.Body,
				AssociatedPostPublicationDate = post.Created
			};

			_favoriteRepository.Save(favorite);
		}
	}
}