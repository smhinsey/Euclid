using System;
using ConsoleBrowserObjects;
using Euclid.Common.HostingFabric;
using Microsoft.Practices.ServiceLocation;

namespace AgentConsole
{
	public class ConsoleFabric : BasicFabric
	{
		public ConsoleFabric(IServiceLocator container) : base(container)
		{
		}

		public override void Start()
		{
			var mainForm = ConsoleForm.GetFormInstance(@".\Forms\Main.xml");

			mainForm.Render();

			base.Start();
		}

		public void ShowError(Exception e)
		{
			var errorForm = ConsoleForm.GetFormInstance(@".\Forms\Error.xml");

			errorForm.Labels["errorTitle"].Text = string.Format("Error Initializing Fabric: {0}", e.Message);

			var errorLineNo = 7;
			
			foreach (var errorLine in extractLinesFromText(e.ToString()))
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
			// split them, insert a word-wrap text character (some sort of turning arrow icon)
			return text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
		}
	}
}