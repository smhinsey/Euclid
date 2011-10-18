namespace Euclid.Framework.EventSourcing
{
	public interface ICanApplyEvent<in TEvent>
		where TEvent : IEvent
	{
		void Apply(TEvent eventToApply);
	}
}