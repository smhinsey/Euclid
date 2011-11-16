using System;
using System.Linq;
using System.Web.Mvc;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Binders;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class CompositeAppExtensions
	{
		public static IInputModel GetInputModelFromCommandName(this ICompositeApp composite, string commandName, IValueProvider valueProvider)
		{
			var inputModelType = composite.GetInputModelTypeForCommandName(commandName);

			var commandType = composite.Commands.Where(x => x.Name == commandName).Select(x => x.Type).FirstOrDefault();

			var inputModel = Activator.CreateInstance(inputModelType) as IInputModel;

			if (inputModel == null)
			{
				throw new CannotCreateInputModelException(inputModelType, commandName);
			}

			foreach (var property in inputModel.GetType().GetProperties())
			{
				var propValue = valueProvider.GetValue(property.Name);
				try
				{
					var value = (property.Name == "CommandType")
									? commandType
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