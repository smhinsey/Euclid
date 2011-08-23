using System;
using Euclid.Common.Logging;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.Sdk.TestAgent.ReadModels;

namespace Euclid.Sdk.TestAgent.Processors
{
	public class TestCommandProcessor : DefaultCommandProcessor<TestCommand>, ILoggingSource
	{
		private readonly TestQuery _query;
		private readonly ISimpleRepository<TestReadModel> _repository;

		public TestCommandProcessor(TestQuery query, ISimpleRepository<TestReadModel> repository)
		{
			_query = query;
			_repository = repository;
		}

		public override void Process(TestCommand message)
		{
			this.WriteInfoMessage("Command no. {0} was processed by TestCommandProcessor", message.Number);

			var model = new TestReadModel
			            	{
			            		Identifier = Guid.NewGuid(),
			            		Number = message.Number,
			            		Created = DateTime.Now,
			            		Modified = DateTime.Now
			            	};

			_repository.Save(model);
		}
	}
}