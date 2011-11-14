using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Binders;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class CompositeAppExtensions
	{
		public static IInputModel GetInputModelFromCommandName(this ICompositeApp app, string commandName, IValueProvider valueProvider)
		{
			var map = Mapper.GetAllTypeMaps().Where(t => t.DestinationType.Name == commandName).FirstOrDefault();

			if (map == null)
			{
				throw new CannotMapCommandException(commandName, app.InputModels.Select(m=>m.Type.FullName));
			}

			var inputModelType = map.SourceType;

			var inputModel = Activator.CreateInstance(inputModelType) as IInputModel;

			if (inputModel == null)
			{
				throw new CannotCreateInputModelException(inputModelType.Name);
			}

			foreach (var property in inputModel.GetType().GetProperties())
			{
				var propValue = valueProvider.GetValue(property.Name);
				try
				{
					var value = (property.Name == "CommandType")
									? map.DestinationType
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