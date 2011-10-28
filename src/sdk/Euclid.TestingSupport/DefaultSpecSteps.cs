using System;
using Castle.Windsor;
using Euclid.Common.Messaging;
using TechTalk.SpecFlow;

namespace Euclid.TestingSupport
{
	/// <summary>
	/// 	A base class for use by steps defined in order to support Specflow based testing of agents.
	/// </summary>
	public abstract class DefaultSpecSteps
	{
		public static Guid PubIdOfLastMessage;

		private const string ContainerContextKey = "IWindsorContainer";

		public static void SetContainerInScenarioContext(IWindsorContainer container)
		{
			ScenarioContext.Current.Add(ContainerContextKey, container);
		}

		protected IWindsorContainer GetContainer()
		{
			return (IWindsorContainer)ScenarioContext.Current[ContainerContextKey];
		}
	}
}