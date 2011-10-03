using System;
using AutoMapper;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.InputModelConversion
{
	[Binding]
	public class InputModelSteps
	{
		ICompositeApp Composite
		{
			get { return ScenarioContext.Current["ca"] as ICompositeApp; }
			set { ScenarioContext.Current["ca"] = value; }
		}

		public InputModelSteps()
		{
			Composite = new BasicCompositeApp(new WindsorContainer());
		}

		[Given(@"a registered inputmodel and command")]
		public void RegisteredCommandAndInputModel()
		{
			//TODO: implement arrange (recondition) logic
			// For storing and retrieving scenario-specific data, 
			// the instance fields of the class or the
			//     ScenarioContext.Current
			// collection can be used.
			// To use the multiline text or the table argument of the scenario,
			// additional string/Table parameters can be defined on the step definition
			// method. 
			Assert.NotNull(Composite);

			Composite.RegisterInputModelMap<TestInputModel, TestCommand>();
		}

		[When(@"GetCommandMetadataForInputModel is called")]
		public void WhenIPressAdd()
		{
			//TODO: implement act (action) logic
			Assert.NotNull(Composite);

			//var model = new TestInputModel
			//                {
			//                    AgentSystemName = typeof (TestInputModel).Assembly.GetAgentMetadata().SystemName,
			//                    CommandType = typeof (TestCommand),
			//                    Number = 8
			//                };

			try
			{
				ScenarioContext.Current["command"] = Composite.GetCommandMetadataForInputModel(typeof(TestInputModel));
			}
			catch (AutoMapperMappingException ex)
			{
				Console.WriteLine(ex.Message);
				Assert.Fail();
			}
		}

		[Then(@"the command is returned")]
		public void ThenTheResultShouldBe()
		{
			//TODO: implement assert (verification) logic

			var metadata = ScenarioContext.Current["command"] as IPartMetadata;

			Assert.NotNull(metadata);

			Assert.AreEqual(typeof (TestCommand), metadata.Type);
		}

		[When(@"the same inputmodel is registered for a new command")]
		public void SameInputModelIsRegistered()
		{
			try
			{
				Composite.RegisterInputModelMap<TestInputModel, SpecCommand>();
			}
			catch (Exception e)
			{
				ScenarioContext.Current["ex"] = e;
			}
		}

		[Then(@"a InputModelAlreadyRegisteredException exception is thrown")]
		public void ExceptionThrown()
		{
			var e = ScenarioContext.Current["ex"] as InputModelAlreadyRegisteredException;

			Assert.NotNull(e);
		}

		[When(@"a new inputmodel is registered for an existing command")]
		public void SameCommand()
		{
			try
			{
				Composite.RegisterInputModelMap<SpecInputModel, TestCommand>();
			}
			catch (Exception e)
			{
				ScenarioContext.Current["ex"] = e;
			}
		}

		[Then(@"a CommandAlreadyMappedException exception is thrown")]
		public void CommandExceptionThrown()
		{
			var e = ScenarioContext.Current["ex"] as CommandAlreadyMappedException;

			Assert.NotNull(e);
		}
	}
}
