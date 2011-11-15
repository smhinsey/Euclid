using System;
using System.Collections.Generic;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.Sdk.TestAgent.ReadModels;
using Euclid.TestingSupport;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.CompositeApplication
{
	[Binding]
	[StepScope(Feature = "Publish input models as commands")]
	public class InputModelSteps : DefaultAgentSteps, IValidateListOfReadModels<TestQuery, TestReadModel>
	{
		public InputModelSteps()
		{
			Initialize();
		}

		protected override Type TypeFromAgent
		{
			get
			{
				return typeof(TestCommand);
			}
		}

		public void ValidateList(TestQuery query, IList<TestReadModel> readModels)
		{
			Assert.Greater(readModels.Count, 0);
		}
	}
}