using Euclid.Composites.Mvc.Models;
using Euclid.Sdk.TestAgent.Commands;

namespace Euclid.Sdk.TestComposite.Models
{
	public class TestInputModel : DefaultInputModel
	{
		public TestInputModel()
		{
			CommandType = typeof (TestCommand);
		}

		public int Number { get; set; }
	}
}