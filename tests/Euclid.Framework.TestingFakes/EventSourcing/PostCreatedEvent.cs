using Euclid.Framework.EventSourcing;

namespace Euclid.Framework.TestingFakes.EventSourcing
{
	public class PostCreatedEvent : DefaultEvent
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public string AuthorUsername { get; set; }
	}
}