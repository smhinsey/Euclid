using Euclid.Framework.Cqrs;

namespace CompositeInspector.Models
{
	public class PublishedCommandModel
	{
		public bool HasValue { get; set; }

		public string PublicationId { get; set; }

		public ICommandPublicationRecord Record { get; set; }
	}
}