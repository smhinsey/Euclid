using Euclid.Framework.Models;

namespace Euclid.Framework.TestingFakes.Cqrs
{
	public class FakeReadModel : DefaultReadModel
	{
		public virtual string Message { get; set; }
	}
}