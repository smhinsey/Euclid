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
					object typedValue = null;
					if (propertyInfo.PropertyType == typeof(Guid))
					{
						typedValue = Guid.Parse((string)form[key]);
					}
					else if (propertyInfo.PropertyType == typeof(Type))
					{
						continue;
					}
					else if (propertyInfo.PropertyType != typeof(HttpPostedFileBase))
					{
						typedValue = Convert.ChangeType((object)form[key], propertyInfo.PropertyType);
					}

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
}