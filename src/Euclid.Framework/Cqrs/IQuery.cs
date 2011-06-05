using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// A query represents a request for one or more instances of a particular type
	/// of IReadModel.
	/// </summary>
	public interface IQuery<TReadModel>
		where TReadModel : IReadModel
	{
		TReadModel GetByIdentifier(Guid identifier);
		IList<TReadModel> GetList(int count, int skip);
	}
}