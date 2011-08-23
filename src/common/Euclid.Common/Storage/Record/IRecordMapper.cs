using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Record
{
	/// <summary>
	/// A record mapper manages basic persistence for an IRecord instance.
	/// </summary>
	/// <typeparam name="TRecord">The record to persist.</typeparam>
	public interface IRecordMapper<TRecord>
	{
		/// <summary>
		/// Create a new persistent record.
		/// </summary>
		/// <param name="record">An unpersisted record.</param>
		/// <returns>The persistent record.</returns>
		TRecord Create(TRecord record);

		/// <summary>
		/// Deletes a persistent record.
		/// </summary>
		/// <param name="id">The record to delete.</param>
		/// <returns>The deleted record.</returns>
		TRecord Delete(Guid id);

		/// <summary>
		/// Returns a list of records visible to the mapper, constrained by count and offset.
		/// </summary>
		/// <param name="count">The maximum number of records to return.</param>
		/// <param name="offset">The number of records to skip before returning results.</param>
		/// <returns>A list of records constrained by count and offset.</returns>
		IList<TRecord> List(int count, int offset);

		/// <summary>
		/// Retrieves an individual record by identifier.
		/// </summary>
		/// <param name="id">The record's identifier.</param>
		/// <returns>The record.</returns>
		TRecord Retrieve(Guid id);

		/// <summary>
		/// Updates a record and returns the updated copy.
		/// </summary>
		/// <param name="record">The record to update.</param>
		/// <returns>The updated record.</returns>
		TRecord Update(TRecord record);
	}
}