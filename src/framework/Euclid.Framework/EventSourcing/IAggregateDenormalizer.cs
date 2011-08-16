using System.Collections.Generic;
using Euclid.Framework.Models;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An aggregate denormalizer converts an instance of an IEventSourcedAggregate into a series of one 
	/// 	or more IReadModels. The denormalizer is invoked whenever an IEventSourcedAggregate instance is 
	/// 	persisted.
	/// </summary>
	public interface IAggregateDenormalizer<in TAggregate>
		where TAggregate : IEventSourcedAggregate
	{
		/// <summary>
		/// Called when a new instance of the supplied aggreate is created or when an existing instance is updated.
		/// </summary>
		/// <param name="aggregate"></param>
		/// <returns>Save returns the read models reflecting the saved aggregate.</returns>
		IList<IReadModel> Save(TAggregate aggregate);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aggregate"></param>
		/// <returns></returns>
		IList<IReadModel> Delete(TAggregate aggregate);
	}
}