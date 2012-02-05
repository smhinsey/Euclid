using Euclid.Framework.Cqrs;
using StorefrontAgent.Commands;

namespace StorefrontAgent.Processors
{
	public class RegisterNewCompanyProcessor : DefaultCommandProcessor<RegisterNewCompany>
	{
		public override void Process(RegisterNewCompany message)
		{
			throw new System.NotImplementedException();
		}
	}
}