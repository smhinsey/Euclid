using System;
using Castle.Windsor;
using TechTalk.SpecFlow;

namespace Euclid.TestingSupport
{
	/// <summary>
	/// A base class for use by steps defined in order to support Specflow based testing of agents.
	/// </summary>
	public abstract class DefaultSpecSteps
	{
		private const string ContainerContextKey = "IWindsorContainer";

		public static Guid PubIdOfLastMessage;

		protected IWindsorContainer GetContainer()
		{
			return (IWindsorContainer) ScenarioContext.Current[ContainerContextKey];
		}

		public static void SetContainerInScenarioContext(IWindsorContainer container)
		{
			ScenarioContext.Current.Add(ContainerContextKey, container);
		}
	}
}