using Euclid.Composites.Mvc.Models;
using Euclid.Sdk.TestAgent.Commands;

namespace Euclid.Sdk.TestComposite.Models
{
	public class ComplexInputModel : DefaultInputModel
	{
		public ComplexInputModel()
		{
			CommandType = typeof (ComplexCommand);
		}

		public string StringValue { get; set; }
	}
}