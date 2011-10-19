using Euclid.Framework.EventSourcing;

namespace Euclid.Framework.TestingFakes.EventSourcing.WriteModel
{
	public class Post : DefaultEventSourcedAggregate
	{
		public virtual string Title { get; set; }
		public virtual string Body { get; set; }
		public virtual User Author { get; set; }
		public virtual int Score { get; set; }
	}
}