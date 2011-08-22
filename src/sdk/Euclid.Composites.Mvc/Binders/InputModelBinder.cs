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
		private readonly IAgentResolver[] _resolvers;

		private readonly IInputModelTransfomerRegistry _transformers;

		public InputModelBinder(IAgentResolver[] resolvers, IInputModelTransfomerRegistry transfomers)
		{
			this._resolvers = resolvers;
			this._transformers = transfomers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var commandName = controllerContext.GetPartName();

			IInputModel inputModel;
			try
			{
				inputModel = this._transformers.GetInputModel(commandName);
			}
			catch (InputModelForPartNotRegisteredException e)
			{
				return null;
			}

			var inputModelProperties = inputModel.GetType().GetProperties();
			foreach (var property in inputModelProperties)
			{
				var propValue = bindingContext.ValueProvider.GetValue(property.Name);
				try
				{
					var value = (property.Name == "CommandType")
					            	? this._transformers.GetCommandType(commandName)
					            	: (propValue == null) ? null : propValue.ConvertTo(property.PropertyType);

					if (property.CanWrite)
					{
						property.SetValue(inputModel, value, null);
					}
				}
				catch (Exception e)
				{
					throw new CannotSetInputModelPropertyValues(inputModel.GetType().Name, property.Name, e);
				}
			}

			return inputModel;
		}

		public bool IsMatch(Type modelType)
		{
			return typeof(IInputModel).IsAssignableFrom(modelType);
		}
	}
}