using System.Collections.Generic;
using Euclid.Framework.Models;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An aggregate denormalizer converts an instance of an IEventSourcedAggregate into a series of one 
	/// 	or more IReadModels. The denormalizer is invoked whenever an IEventSourcedAggregate instance is 
	/// 	persisted.
	/// </summary>
	/// <typeparam name="TAggregate">The aggregate to denormalize.</typeparam>
	public interface IAggregateDenormalizer<in TAggregate>
		where TAggregate : IEventSourcedAggregate
	{
		/// <summary>
		/// 	Called when an aggregate is deleted.
		/// </summary>
		/// <param name = "aggregate">Aggregate to be deleted.</param>
		/// <returns>A list of read models which should be deleted.</returns>
		IList<IReadModel> Delete(TAggregate aggregate);

		/// <summary>
		/// 	Called when a new instance of the supplied aggreate is created or when an existing instance is updated.
		/// </summary>
		/// <param name = "aggregate">Aggregate to be saved.</param>
		/// <returns>A list of read models reflecting the saved aggregate.</returns>
		IList<IReadModel> Save(TAggregate aggregate);
	}
}