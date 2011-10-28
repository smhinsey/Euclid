using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Euclid.TestingSupport
{
	[Binding]
	public abstract class DefaultAgentSteps
	{
		private const string LastPublicationKey = "LastPublicationRecord";
		private const string LastPublicationIdentifierKey = "LastPublicationIdentifier";
		private const string AgentMetadataKey = "AgentMetadata";
		private const string ContainerKey = "IWindsorContainer";
		private const string ConfiguratorKey = "AgentConfigurator";
		private const string InitializedKey = "Initialized";
		private const string PreviousCommandKey = "PreviousCommand";
		private const string PreviousReadModelKey = "PreviousReadModel";

		public AgentConfigurator Configurator
		{
			get { return (AgentConfigurator) ScenarioContext.Current[ConfiguratorKey]; }
			set { ScenarioContext.Current[ConfiguratorKey] = value; }

		}

		private Type CommandStep { get; set; }

		protected IAgentMetadata AgentMetadata
		{
			get { return ScenarioContext.Current[AgentMetadataKey] as IAgentMetadata; }
			set { ScenarioContext.Current[AgentMetadataKey] = value; }
		}

		private IPublicationRecord LastPublication
		{
			get { return ScenarioContext.Current[LastPublicationKey] as IPublicationRecord; }
			set { ScenarioContext.Current[LastPublicationKey] = value; }
		}

		private Guid LastPublicationIdentifier
		{
			get { return (Guid) ScenarioContext.Current[LastPublicationIdentifierKey]; }
			set { ScenarioContext.Current[LastPublicationIdentifierKey] = value; }
		}

		private bool Initialized
		{
			get { return (bool) ScenarioContext.Current[InitializedKey]; }
			set { ScenarioContext.Current[InitializedKey] = value; }
		}

		protected ICommand PreviousCommand
		{
			get { return (ICommand) ScenarioContext.Current[PreviousCommandKey]; }
			set { ScenarioContext.Current[PreviousCommandKey] = value; }
		}

		protected IReadModel PreviousReadModel
		{
			get { return (IReadModel) ScenarioContext.Current[PreviousReadModelKey]; }
			set { ScenarioContext.Current[PreviousReadModelKey] = value; }
		}

		protected abstract Type TypeFromAgent { get; }

		protected IWindsorContainer Container
		{
			get { return (IWindsorContainer) ScenarioContext.Current[ContainerKey]; }
			set { ScenarioContext.Current[ContainerKey] = value; }
		}

		public void Initialize()
		{
			Container = new WindsorContainer();
			var agentAssembly = TypeFromAgent.Assembly;

			Configurator = new AgentConfigurator(Container);
			Configurator.Configure(agentAssembly);

			AgentMetadata = agentAssembly.GetAgentMetadata();

			Initialized = true;
		}

		[Given(@"the agent (.*)")]
		public void GivenTheAgent(string assemblyName)
		{
			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			Assert.IsNotNull(AgentMetadata, "Could not retrieve metadadata for the agent in assembly '{0}'", assemblyName);

			Assert.IsTrue(AgentMetadata.IsValid, "The assembly '{0}' contains invalid agent metadata", assemblyName);

			AgentInitialized(AgentMetadata);
		}

		[When(@"I publish the command (.*):")]
		public void PublishCommand(string commandName, Table table)
		{
			var commandType = AgentMetadata.GetPartByTypeName(commandName).Type;

			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			var commandInstance = GetInstanceFromTable<ICommand>(commandType, table);

			CommandStep = GetType()
				.GetInterfaces()
				.Where(
					i =>
					i.Name == "ICommandCompleteStep`1" && i.IsGenericType && i.GetGenericArguments()[0] == commandInstance.GetType())
				.FirstOrDefault();

			var publishCommand = GetType()
				.GetInterfaces()
				.Where(
					i =>
					i.Name == "ICommandPublishStep`1" && i.IsGenericType && i.GetGenericArguments()[0] == commandInstance.GetType())
				.FirstOrDefault();

			if (publishCommand != null)
			{
				var method = publishCommand.GetMethod("GetCommand");

				try
				{
					commandInstance = method.Invoke(this, new object[] { commandInstance }) as ICommand;
				}
				catch (Exception ex)
				{
					PreserveStackTrace(ex.InnerException);

					throw ex.InnerException;
				}
			}

			var publisher = Container.Resolve<IPublisher>();

			LastPublicationIdentifier = publisher.PublishMessage(commandInstance);

			PreviousCommand = commandInstance;
		}

		[When(@"the command is complete")]
		public void WhenTheCommandIsComplete()
		{
			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			var attempts = 0;

			do
			{
				var registry = Container.Resolve<ICommandRegistry>();

				LastPublication = registry.GetPublicationRecord(LastPublicationIdentifier);

				if (LastPublication.Completed || LastPublication.Error)
				{
					attempts = 15;
				}
				else
				{
					Thread.Sleep(350);
				}

				attempts++;
			} while (attempts < 15);

			if (CommandStep == null)
			{
				CommandCompleted(LastPublication, PreviousCommand);
			}
			else
			{
				var method = CommandStep.GetMethod("CommandCompleted");

				try
				{
					method.Invoke(this, new object[] { LastPublication, PreviousCommand });
				}
				catch (Exception ex)
				{
					PreserveStackTrace(ex.InnerException);

					throw ex.InnerException;
				}
			}
		}

		[Then(@"run (.*) on (.*) with:")]
		public void RunQuery(string methodName, string queryPartTypeName, Table table)
		{
			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			var queryPartType = AgentMetadata.GetPartByTypeName(queryPartTypeName).Type;

			List<object> methodParams;

			var queryMethod = GetQuery(methodName, queryPartType, table, out methodParams);

			object queryResults;

			try
			{
				queryResults = queryMethod.Invoke(Container.Resolve(queryPartType), methodParams.ToArray());
			}
			catch (Exception ex)
			{
				PreserveStackTrace(ex.InnerException);

				throw ex.InnerException;
			}

			var readModel =  queryResults as IReadModel;

			Assert.NotNull(readModel, "The query {0}.{1} did not return a read model", queryPartType.Name, methodName);

			PreviousReadModel = readModel;
		}

		[Then(@"retrieve a List of (.*) by running (.*) on (.*) with:")]
		public void QueryForListOfResults(string readModelParTypeName, string methodName, string queryPartTypeName, Table table)
		{
			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			var queryPartType = AgentMetadata.GetPartByTypeName(queryPartTypeName).Type;

			List<object> methodParams;

			var queryMethod = GetQuery(methodName, queryPartType, table, out methodParams);

			object queryResults;

			try
			{
				queryResults = queryMethod.Invoke(Container.Resolve(queryPartType), methodParams.ToArray());
			}
			catch (Exception ex)
			{
				PreserveStackTrace(ex.InnerException);

				throw ex.InnerException;
			}

			var readModelType = AgentMetadata.GetPartByTypeName(readModelParTypeName).Type;

			var validateListOfReadModels = GetType()
											.GetInterfaces()
											.Where(
												i =>
												i.Name == "IValidateListOfReadModels`2" && i.GetGenericArguments()[0] == queryPartType && i.GetGenericArguments()[1] == readModelType)
											.FirstOrDefault(); 

			Assert.NotNull(validateListOfReadModels, "Could not retrieve an IValidateListOfReadModels<{0}, {1}>", queryPartTypeName, readModelType.Name);
			
			var listValidationMethod = validateListOfReadModels.GetMethod("ValidateList");

			try
			{
				listValidationMethod.Invoke(this, new[] { Container.Resolve(queryPartType), queryResults });
			}
			catch (Exception ex)
			{
				PreserveStackTrace(ex.InnerException);

				throw ex.InnerException;
			}
		}

		[Then(@"the (.*) has values:")]
		public void ReadModelHasValues(string readModelPartTypeName, Table table)
		{
			Assert.IsTrue(Initialized, "DefaultAgentSteps.Initialize must be called before steps can run");

			Assert.NotNull(PreviousReadModel, "Cannot validate the {0} properties, the read model hasn't been saved", readModelPartTypeName);

			var readModelType = AgentMetadata.GetPartByTypeName(readModelPartTypeName).Type;

			Assert.NotNull(readModelType, "Could not find the read model {0} in agent {1}", readModelPartTypeName, AgentMetadata.DescriptiveName);

			Assert.AreEqual(PreviousReadModel.GetType(), readModelType, "Expected read model type {0}, not the specified read model type {1}", PreviousReadModel.GetType(), readModelPartTypeName);

			var readModel = GetInstanceFromTable<IReadModel>(PreviousReadModel.GetType(), table);

			foreach (var name in table.Header)
			{
				var prop = readModel.GetType().GetProperty(name);
				if (prop.CanRead)
				{
					var expectedValue = prop.GetValue(readModel, null);
					var actualValue = prop.GetValue(PreviousReadModel, null);
					var errorMessage = string.Format("{0}.{1}", readModelType.Name, prop.Name);
					Assert.AreEqual(expectedValue, actualValue, errorMessage);
				}
			}
		}

		protected virtual void AgentInitialized(IAgentMetadata agentMetadata)
		{

		}

		protected void CommandCompleted(IPublicationRecord publicationRecord, ICommand previousCommand)
		{
			Console.WriteLine("{0} Command Completed", previousCommand.GetType().Name);

			Assert.IsTrue(publicationRecord.Completed, "The publication did not complete");

			Assert.IsFalse(publicationRecord.Error, "An error occurred while processing the command: \r\n\t{0}", publicationRecord.ErrorMessage);
		}

		protected T GetInstanceFromTable<T>(Type agentPartType, Table table)
			where T : class, IAgentPart
		{
			var method =
				typeof (TableHelperExtensionMethods).GetMethods().Where(
					m => m.Name == "CreateInstance" && m.GetParameters().Count() == 1).First();

			var generic = method.MakeGenericMethod(agentPartType);

			T agentPartInstance;
			try
			{
				agentPartInstance = generic.Invoke(null, new[] { table }) as T;
			}
			catch (Exception ex)
			{
				PreserveStackTrace(ex.InnerException);

				throw ex.InnerException;
			}

			Assert.NotNull(agentPartInstance, "Could not create an instance of type {0} from the table ({1})", agentPartType.Name, table);

			return agentPartInstance;
		}

		private MethodInfo GetQuery(string methodName, Type queryPartType, Table table, out List<object> methodParams)
		{
			var queryMethod = queryPartType.GetMethod(methodName);

			Assert.NotNull(queryMethod, "Could not find the query named '{0}' in part '{1}'", methodName, queryPartType.Name);

			methodParams = new List<object>();
			var methodParameterInfo = queryMethod.GetParameters();
			foreach (var parameterName in table.Header)
			{
				var parameterInfo = methodParameterInfo.Where(p => p.Name.ToLower() == parameterName.ToLower()).FirstOrDefault();
				Assert.NotNull(parameterInfo,
				               "Could not find parameter named {0} in method {1}.{2}",
				               parameterName,
				               queryMethod.Name,
				               queryMethod.Name);

				var parameterValueAsString = table.Rows[0][parameterName];
				methodParams.Add(parameterInfo.ParameterType == typeof (Guid)
				                 	? new Guid(parameterValueAsString)
									: Convert.ChangeType(parameterValueAsString, parameterInfo.ParameterType));
			}

			Assert.AreEqual(methodParameterInfo.Count(),
			                methodParams.Count,
			                "The query {0}.{1} expects {2} parameters but {3} were specified",
			                queryMethod.Name,
			                queryMethod.Name,
			                methodParameterInfo.Count(),
			                methodParams.Count);

			return queryMethod;
		}

		private static void PreserveStackTrace(Exception exception)
		{
			var preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

			preserveStackTrace.Invoke(exception, null);
		}
	}
}