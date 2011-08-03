using System;
using System.Collections.Generic;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A query represents a request for one or more instances of a particular type
	/// 	of IReadModel. Typically, a query will be backed by a repository, but this
	/// 	decision is up in the hands of the query implementor.
	/// </summary>
	public interface IQuery<TReadModel> : IAgentPart
		where TReadModel : IReadModel
	{
		IList<TReadModel> FindByCreationDate(DateTime specificDate);
		IList<TReadModel> FindByCreationDate(DateTime begin, DateTime end);
		TReadModel FindById(Guid id);
		IList<TReadModel> FindByModificationDate(DateTime specificDate);
		IList<TReadModel> FindByModificationDate(DateTime begin, DateTime end);
	}
}