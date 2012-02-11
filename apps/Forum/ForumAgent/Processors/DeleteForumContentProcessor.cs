using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class DeleteForumContentProcessor : DefaultCommandProcessor<DeleteForumContent>
	{
		private readonly ISimpleRepository<ForumContent> _contentRepository;

		public DeleteForumContentProcessor(ISimpleRepository<ForumContent> contentRepository)
		{
			_contentRepository = contentRepository;
		}

		public override void Process(DeleteForumContent message)
		{
			_contentRepository.Delete(message.ContentIdentifier);
		}
	}
}