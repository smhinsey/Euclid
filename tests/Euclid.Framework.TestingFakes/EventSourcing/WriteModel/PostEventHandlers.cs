using Euclid.Framework.EventSourcing;

namespace Euclid.Framework.TestingFakes.EventSourcing.WriteModel
{
	// SELF: not yet sure if we want to encourage one handler per class or not 
	public class PostEventHandlers :
		ICanApplyEvent<PostCreatedEvent>
	{
		public void Apply(PostCreatedEvent eventToApply)
		{
			// create the write model, somehow it needs to be tracked so it can be denormalized
		}
	}
}