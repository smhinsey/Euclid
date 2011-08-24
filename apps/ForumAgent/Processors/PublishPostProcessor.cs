﻿using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class PublishPostProcessor : DefaultCommandProcessor<PublishPost>
	{
		private readonly ISimpleRepository<Post> _repository;

		public PublishPostProcessor(ISimpleRepository<Post> repository)
		{
			_repository = repository;
		}

		public override void Process(PublishPost message)
		{
			// SELF at some point we'll want to wire up the actual author name there
			var post = new Post
			           	{
			           		AuthorIdentifier = message.AuthorIdentifier, 
										AuthorDisplayName = "Anonymous",
			           		Body = message.Body, 
			           		Score = 0, 
			           		Title = message.Title, 
			           		CategoryIdentifier = message.CategoryIdentifier, 
			           		Identifier = message.Identifier, 
			           		Created = DateTime.Now, 
			           		Modified = DateTime.Now
			           	};

			_repository.Save(post);
		}
	}
}
