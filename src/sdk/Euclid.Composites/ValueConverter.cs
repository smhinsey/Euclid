using System;
using Euclid.Common.Logging;

namespace Euclid.Composites
{
	public class ValueConverter : ILoggingSource
	{
		private ValueConverter()
		{
			
		}

		public static object GetValueAs(string postedValue, Type expectedType)
		{
			var self = new ValueConverter();

			object typedValue = null;
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
			else if (expectedType != typeof(Type))
			{
				try
				{
					typedValue = Convert.ChangeType(postedValue, expectedType);
				}
				catch (InvalidCastException e)
				{
					typedValue = null;
					self.WriteErrorMessage("Invalid cast request", e.Message);
				}
			}

			return typedValue;
		}

		public static T GetValueAs<T>(string postedValue)
		{
			return (T) GetValueAs(postedValue, typeof (T));
		}
	}
}