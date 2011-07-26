using System;
using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Binders
{
	public interface IEuclidModelBinder : IModelBinder
	{
		bool IsMatch(Type modelType);
	}
}