using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A query represents a request for one or more instances of a particular type
	/// 	of IReadModel.
	/// </summary>
	public interface IQuery<TReadModel>
		where TReadModel : IReadModel
	{
		IList<TReadModel> FindByCreationDate(DateTime specificDate);
		IList<TReadModel> FindByCreationDate(DateTime begin, DateTime end);
		TReadModel FindById(Guid id);
		IList<TReadModel> FindByModificationDate(DateTime specificDate);
		IList<TReadModel> FindByModificationDate(DateTime begin, DateTime end);
	}
}