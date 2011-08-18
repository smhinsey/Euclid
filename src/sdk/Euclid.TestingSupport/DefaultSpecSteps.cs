using System;
using System.Threading;
using Castle.Windsor;
using Euclid.Framework.Cqrs;
using TechTalk.SpecFlow;

namespace Euclid.TestingSupport
{
	public class DefaultSpecSteps
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