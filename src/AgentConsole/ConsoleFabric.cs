using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using ConsoleBrowserObjects;
using Euclid.Agent.Extensions;
using Euclid.Common.HostingFabric;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.HostingFabric;
using Euclid.Framework.Metadata.Attributes;
using Microsoft.Practices.ServiceLocation;

namespace AgentConsole
{
	public class ConsoleFabric : BasicFabric
	{
		private readonly IWindsorContainer _container;
		private BasicCompositeApp _composite;

		public ConsoleFabric(IWindsorContainer container)
			: base(new WindsorServiceLocator(container))
		{
			container.Register(Component.For<IServiceLocator>().Instance(Container));

			_container = container;
		}

		public override void Initialize(IFabricRuntimeSettings settings)
		{
			_container.Register(Component.For<IServiceHost>()
			                    	.Forward<MultitaskingServiceHost>()
			                    	.Instance(new MultitaskingServiceHost()));

			base.Initialize(settings);
		}

		public override void Start()
		{
			var mainForm = ConsoleForm.GetFormInstance(@".\Forms\Main.xml");

			mainForm.Labels["agentCount"].Text = string.Format("Agents: {0}", _composite.Agents.Count);

			mainForm.Render();

			base.Start();
		}

		// SELF this should be pushed down to the base type
		public void InstallComposite(BasicCompositeApp composite)
		{
			if (_composite != null)
			{
				throw new Exception("A composite has already been installed");
			}

			if (composite.State != CompositeApplicationState.Configured)
			{
				throw new Exception("Only configured composites can be installed in the fabric.");
			}

			_composite = composite;

			foreach (var agent in _composite.Agents)
			{
				var processorAttribute = agent.AgentAssembly.GetAttributeValue<LocationOfProcessorsAttribute>();

				_composite.Container.Register
					(AllTypes.FromAssembly(agent.AgentAssembly)
					 	.Where(Component.IsInNamespace(processorAttribute.Namespace))
					 	.BasedOn(typeof (ICommandProcessor))
					 	.WithService.AllInterfaces().WithService.Self());

				var registry = Container.GetInstance<ICommandRegistry>();

				var dispatcher = new CommandDispatcher(Container, registry);

				var dispatcherSettings = new MessageDispatcherSettings();

				dispatcherSettings.InputChannel.WithDefault(CurrentSettings.InputChannel.Value);
				dispatcherSettings.InvalidChannel.WithDefault(CurrentSettings.ErrorChannel.Value);

				var processors = _composite.Container.ResolveAll(typeof (ICommandProcessor));

				foreach (var processor in processors)
				{
					dispatcherSettings.MessageProcessorTypes.Add(processor.GetType());
				}

				dispatcher.Configure(dispatcherSettings);

				var commandHost = new CommandHost(new ICommandDispatcher[] {dispatcher});

				_composite.Container.Register(Component.For<IHostedService>().Instance(commandHost).Forward<CommandHost>());
			}
		}

		public void ShowError(Exception e)
		{
			var errorForm = ConsoleForm.GetFormInstance(@".\Forms\Error.xml");

			errorForm.Labels["errorTitle"].Text = string.Format("Error Initializing Fabric: {0}", e.Message);

			var errorLineNo = 7;

			var linesFromText = extractLinesAndWordWrap(e.ToString());

			foreach (var errorLine in linesFromText)
			{
				var lineLabel = new Label(string.Format("error{0}", errorLineNo), new Point(3, errorLineNo), 122, errorLine);
				errorForm.Labels.Add(lineLabel);
				errorLineNo++;
			}

			errorForm.Render();
		}

		private string[] extractLinesAndWordWrap(string text)
		{
			var lineList = new List<string>(text.Split(new[] {Environment.NewLine}, StringSplitOptions.None));

			var results = new List<string>();

			for (var i = 0; i < lineList.Count; i++)
			{
				var line = lineList[i];

				if (line.Length > 120)
				{
					var left = line.Substring(0, 99);
					var right = line.Substring(99, line.Length - 99);

					results.Add(left);
					results.Add(string.Format("→ {0}", right));
				}
				else
				{
					results.Add(line);
				}
			}

			return results.ToArray();
		}
	}
}