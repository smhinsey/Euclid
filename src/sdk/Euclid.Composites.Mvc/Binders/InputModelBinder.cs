using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Binders
{
	public class InputModelBinder : IEuclidModelBinder
	{
		private readonly IInputModelTransformerRegistry _transformers;

		public InputModelBinder(IInputModelTransformerRegistry transformers)
		{
			_transformers = transformers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var commandName = controllerContext.GetPartName();
		    var valueProvider = bindingContext.ValueProvider;

			return _transformers.GetInputModel(commandName, valueProvider);
		}

	    public bool IsMatch(Type modelType)
		{
			return typeof (IInputModel).IsAssignableFrom(modelType);
		}
	}
}
