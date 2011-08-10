using System;
using Castle.MicroKernel.Registration;
using ConsoleBrowserObjects;
using Euclid.Agent.Extensions;
using Euclid.Common.HostingFabric;
using Euclid.Composites;
using Euclid.Framework.Metadata.Attributes;
using Microsoft.Practices.ServiceLocation;

namespace AgentConsole
{
	public class ConsoleFabric : BasicFabric
	{
		private BasicCompositeApp _composite;

		public ConsoleFabric(IServiceLocator container) : base(container)
		{
		}

		public override void Start()
		{
			var mainForm = ConsoleForm.GetFormInstance(@".\Forms\Main.xml");

			mainForm.Render();

			base.Start();
		}

		// SELF this should be pushed down to the base type, which should probably be moved from Common to Framework
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
					 	.WithService.AllInterfaces());
			}
		}

		public void ShowError(Exception e)
		{
			var errorForm = ConsoleForm.GetFormInstance(@".\Forms\Error.xml");

			errorForm.Labels["errorTitle"].Text = string.Format("Error Initializing Fabric: {0}", e.Message);

			var errorLineNo = 7;

			var linesFromText = extractLinesFromText(e.ToString());

			foreach (var errorLine in linesFromText)
			{
				var lineLabel = new Label(string.Format("error{0}", errorLineNo), new Point(3, errorLineNo), 122, errorLine);
				errorForm.Labels.Add(lineLabel);
				errorLineNo++;
			}

			errorForm.Render();
		}

		private string[] extractLinesFromText(string text)
		{
			// SELF add a basic word-wrapping algorithm. 
			// iterate the results prior to returning, detect lines wider than a max width constant
			// split them, insert a suitable ASCII character (some sort of arrow)
			return text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
		}
	}
}