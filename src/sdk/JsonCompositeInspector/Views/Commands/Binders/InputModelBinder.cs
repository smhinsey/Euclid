using System;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Framework.Models;
using Nancy;
using Nancy.ModelBinding;

namespace JsonCompositeInspector.Views.Command.Binders
{
	public class InputModelBinder : IModelBinder
	{
		private readonly ICompositeApp _compositeApp;
		private readonly IWindsorContainer _container;

		public InputModelBinder(ICompositeApp compositeApp, IWindsorContainer container)
		{
			_compositeApp = compositeApp;
			_container = container;
		}

		public object Bind(NancyContext context, Type modelType, params string[] blackList)
		{
			var systemName = ((string)context.Request.Form["AgentSystemName"]).Trim();
			var partName = ((string)context.Request.Form["PartName"]).Trim();

			var inputModelType = _compositeApp.GetInputModelTypeForCommandName(partName);
			var inputModel = _container.Resolve<IInputModel>(inputModelType.Name);

			var properties = inputModelType.GetProperties();
			var form = context.Request.Form as DynamicDictionary;
			if (form == null) return null;
			foreach (var key in form)
			{
				var formName = key;
				var propertyInfo = properties
									.Where(p => p.Name.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && p.CanWrite)
									.FirstOrDefault();

				if (propertyInfo != null && form[key] != null)
				{
					var postedValue = (string)form[key];
					var expectedType = propertyInfo.PropertyType;
					object typedValue = ValueConverter.GetValueAs(postedValue, expectedType);
					propertyInfo.SetValue(inputModel, typedValue, null);
				}
			}

			return inputModel;
		}

		public bool CanBind(Type modelType)
		{
			return typeof(IInputModel).IsAssignableFrom(modelType);
		}
	}

	public static class ValueConverter
	{
		public static object GetValueAs(string postedValue, Type expectedType)
		{
			object typedValue = null;
			if (
						expectedType == typeof(HttpPostedFileBase)
						||
						expectedType == typeof(Type))
			{
				return null;
			}

			if (expectedType == typeof(Guid))
			{
				typedValue = Guid.Parse(postedValue);
			}
			else if (expectedType.IsEnum)
			{
				typedValue = Enum.Parse(expectedType, postedValue);
			}
			else if (expectedType == typeof(Boolean))
			{
				bool boolValue;
				if (!Boolean.TryParse(postedValue, out boolValue))
				{
					boolValue = postedValue.Equals("on", StringComparison.InvariantCultureIgnoreCase);
				}

				typedValue = boolValue;
			}
			else if (expectedType == typeof(DateTime))
			{
				typedValue = DateTime.Parse(postedValue);
			}
			else if (expectedType != typeof(HttpPostedFileBase))
			{
				typedValue = Convert.ChangeType(postedValue, expectedType);
			}

			return typedValue;
		}
	}
}