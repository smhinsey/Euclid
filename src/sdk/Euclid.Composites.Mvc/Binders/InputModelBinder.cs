using System;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Binders
{
	public class InputModelBinder : IEuclidModelBinder
	{
		private readonly ICompositeApp _composite;

		public InputModelBinder(ICompositeApp composite)
		{
			_composite = composite;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var commandName = controllerContext.GetPartName();

			var valueProvider = bindingContext.ValueProvider;

			return _composite.GetInputModelFromCommandName(commandName, valueProvider, controllerContext.HttpContext.Request.Files);
		}

		public bool IsMatch(Type modelType)
		{
			return typeof(IInputModel).IsAssignableFrom(modelType);
		}
	}
}