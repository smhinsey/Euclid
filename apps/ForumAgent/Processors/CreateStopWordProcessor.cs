using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateStopWordProcessor : DefaultCommandProcessor<CreateStopWord>
	{
		private readonly ISimpleRepository<StopWord> _stopWordRepository;

		public CreateStopWordProcessor(ISimpleRepository<StopWord> stopWordRepository)
		{
			_stopWordRepository = stopWordRepository;
		}

		public override void Process(CreateStopWord message)
		{
			_stopWordRepository.Save(
				new StopWord
					{
						Active = message.Active,
						Created = DateTime.Now,
						ForumIdentifier = message.ForumIdentifier,
						Modified = (DateTime)SqlDateTime.MinValue,
						Identifier = Guid.NewGuid(),
						WordToMatch = message.WordToMatch,
						ReplacementWord = message.ReplacementWord
					});
		}
	}
}