using System;
using System.Web;

namespace JsonCompositeInspector.Views.Commands.Binders
{
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

		public static T GetValueAs<T>(string postedValue)
		{
			return (T) GetValueAs(postedValue, typeof (T));
		}
	}
}