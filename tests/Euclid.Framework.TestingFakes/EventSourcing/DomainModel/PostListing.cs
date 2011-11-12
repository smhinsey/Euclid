using Euclid.Common.Messaging;

namespace Euclid.Framework.TestingFakes.EventSourcing.DomainModel
{
	public class PostListing : MultipleMessageProcessor
	{
		// aggregate root for Post, handles all relevant commands
		
		public void CreatePost(CreatePostCommand createPost)
		{
			// save the write model
			
			// convert to PostCreatedEvent and publish it
		}
	}
}