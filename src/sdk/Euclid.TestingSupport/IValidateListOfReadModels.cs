using System.Collections.Generic;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.TestingSupport
{
	public interface IValidateListOfReadModels<in TQuery, TReadModel>
		where TQuery : IQuery<TReadModel> 
		where TReadModel : IReadModel
	{
		void ValidateList(TQuery query, IList<TReadModel> readModels);
	}
}