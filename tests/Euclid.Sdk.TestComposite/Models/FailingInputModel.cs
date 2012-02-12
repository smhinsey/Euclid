using Euclid.Composites.Mvc.Models;
using Euclid.Sdk.TestAgent.Commands;

namespace Euclid.Sdk.TestComposite.Models
{
	public class FailingInputModel : DefaultInputModel
	{
		public FailingInputModel()
		{
			CommandType = typeof(FailingCommand);
		}
	}
}