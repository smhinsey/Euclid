using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Binders
{
	// lifted wholesale from http://lostechies.com/jimmybogard/2009/03/18/a-better-model-binder/
	public class EuclidDefaultBinder : DefaultModelBinder
	{
		private readonly IEuclidModelBinder[] _euclidModelBinders;

		public EuclidDefaultBinder(IEuclidModelBinder[] euclidModelBinders)
		{
			_euclidModelBinders = euclidModelBinders;
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			foreach (var binder in _euclidModelBinders)
			{
				if (binder.IsMatch(bindingContext.ModelType))
				{
					return binder.BindModel(controllerContext, bindingContext);
				}
			}

			return (bindingContext.ModelType.IsInterface) ? null : base.BindModel(controllerContext, bindingContext);
		}
	}
}