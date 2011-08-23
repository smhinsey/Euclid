using System;
using Euclid.Common.Logging;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using Euclid.Sdk.FakeAgent.ReadModels;

namespace Euclid.Sdk.FakeAgent.Processors
{
	public class FakeCommandProcessor : DefaultCommandProcessor<FakeCommand>, ILoggingSource
	{
		private readonly FakeQuery _query;

		private readonly ISimpleRepository<FakeReadModel> _repository;

		public FakeCommandProcessor(FakeQuery query, ISimpleRepository<FakeReadModel> repository)
		{
			_query = query;
			_repository = repository;
		}

		public override void Process(FakeCommand message)
		{
			this.WriteInfoMessage("Command no. {0} was processed by FakeCommandProcessor", message.Number);

			var model = new FakeReadModel
				{
       Identifier = Guid.NewGuid(), Number = message.Number, Created = DateTime.Now, Modified = DateTime.Now 
    };

			_repository.Save(model);
		}
	}
}