using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class DeleteStopWordProcessor : DefaultCommandProcessor<DeleteStopWord>
	{
		private readonly ISimpleRepository<StopWord> _stopWordRepository;

		public DeleteStopWordProcessor(ISimpleRepository<StopWord> stopWordRepository)
		{
			_stopWordRepository = stopWordRepository;
		}

		public override void Process(DeleteStopWord message)
		{
			_stopWordRepository.Delete(message.StopWordIdentifier);
		}
	}
}