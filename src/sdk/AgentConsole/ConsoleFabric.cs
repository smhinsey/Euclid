using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using ConsoleBrowserObjects;
using Euclid.Common.ServiceHost;
using Euclid.Framework.HostingFabric;
using Microsoft.Practices.ServiceLocation;

namespace AgentConsole
{
	public class ConsoleFabric : BasicFabric
	{
		public ConsoleFabric(IWindsorContainer container) : base(container)
		{
			container.Register(Component.For<IServiceLocator>().Instance(new WindsorServiceLocator(Container)));
		}

		public override void Initialize(IFabricRuntimeSettings settings)
		{
			Container.Register(Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost()));

			base.Initialize(settings);
		}

		public override void Start()
		{
			var mainForm = ConsoleForm.GetFormInstance(@".\Forms\Main.xml");

			mainForm.Labels["agentCount"].Text = string.Format("Agents: {0}", Composite.Agents.Count);

			// mainForm.Render();

			base.Start();
		}

		public void ShowError(Exception e)
		{
			var errorForm = ConsoleForm.GetFormInstance(@".\Forms\Error.xml");

			errorForm.Labels["errorTitle"].Text = string.Format("Error Initializing Fabric: {0}", e.Message);

			var errorLineNo = 7;

			foreach (var errorLine in extractLinesAndWordWrap(e.ToString()))
			{
				var lineLabel = new Label(string.Format("error{0}", errorLineNo), new Point(3, errorLineNo), 122, errorLine);

				errorForm.Labels.Add(lineLabel);
				errorLineNo++;
			}

			errorForm.Render();
		}

		private string[] extractLinesAndWordWrap(string text)
		{
			const int maxLineLength = 120;
			const int wrappedLineLength = 110;

			var lineList = new List<string>(text.Split(new[] {Environment.NewLine}, StringSplitOptions.None));

			var results = new List<string>();

			foreach (var line in lineList)
			{
				if (line.Length > maxLineLength)
				{
					var left = line.Substring(0, wrappedLineLength);
					var right = line.Substring(wrappedLineLength, line.Length - wrappedLineLength);

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