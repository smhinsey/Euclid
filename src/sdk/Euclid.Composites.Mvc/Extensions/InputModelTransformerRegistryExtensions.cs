using System;
using System.Web.Mvc;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Binders;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class InputModelTransformerRegistryExtensions
	{
		public static IInputModel GetInputModel(
			this IInputModelTransformerRegistry tranformers, string commandName, IValueProvider valueProvider)
		{
			IInputModel inputModel;
			try
			{
				inputModel = tranformers.GetInputModel(commandName);
			}
			catch (InputModelForPartNotRegisteredException e)
			{
				return null;
			}

			var inputModelProperties = inputModel.GetType().GetProperties();
			foreach (var property in inputModelProperties)
			{
				var propValue = valueProvider.GetValue(property.Name);
				try
				{
					var value = (property.Name == "CommandType")
					            	? tranformers.GetCommandType(commandName)
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
	}
}