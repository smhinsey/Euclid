using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateStopWordProcessor : DefaultCommandProcessor<ActivateStopWord>
	{
		private readonly ISimpleRepository<StopWord> _stopWordRepository;

		public ActivateStopWordProcessor(ISimpleRepository<StopWord> stopWordRepository)
		{
			_stopWordRepository = stopWordRepository;
		}

		public override void Process(ActivateStopWord message)
		{
			var stopWord = _stopWordRepository.FindById(message.StopWordIdentifier);

			stopWord.Modified = DateTime.Now;
			stopWord.Active = message.Active;

			_stopWordRepository.Update(stopWord);
		}
	}
}