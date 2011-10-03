using System;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Models;
using Euclid.Composites.Mvc.Extensions;

namespace Euclid.Composites.Mvc.Binders
{
	public class InputModelBinder : IEuclidModelBinder
	{
		private readonly ICompositeApp _compositeApp;

		public InputModelBinder(ICompositeApp compositeApp)
		{
			_compositeApp = compositeApp;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var commandName = controllerContext.GetPartName();

			var valueProvider = bindingContext.ValueProvider;

			return _compositeApp.GetInputModelFromCommandName(commandName, valueProvider);
		}

		public bool IsMatch(Type modelType)
		{
			return typeof(IInputModel).IsAssignableFrom(modelType);
		}
	}
}