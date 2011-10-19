using Euclid.Framework.EventSourcing;

namespace Euclid.Framework.TestingFakes.EventSourcing
{
	public class PostCreatedEvent : DefaultEvent
	{
		public virtual string Title { get; set; }
		public virtual string Body { get; set; }
		public virtual string AuthorUsername { get; set; }
	}
}