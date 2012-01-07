using Euclid.Framework.Cqrs;

namespace JsonCompositeInspector.Models
{
	public class PublishedCommandModel
	{
		public string PublicationId { get; set; }

		public ICommandPublicationRecord Record { get; set; }
	}
}