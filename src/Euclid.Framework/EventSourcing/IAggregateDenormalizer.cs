using System.Collections.Generic;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// An aggregate denormalizer converts an instance of an IAggregateRoot into a series of one 
	/// or more IReadModels. The denormalizer is invoked whenever an IAggregateRoot instance is 
	/// modified and persisted.
	/// </summary>
	public interface IAggregateDenormalizer<in TAggregate>
		where TAggregate : IAggregateRoot
	{
		IList<IReadModel> Denormalize(TAggregate aggregate);
	}
}