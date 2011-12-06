using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public class InputModelFormatter : MetadataFormatter
	{
		private readonly IList<Properties> _properties;


		public InputModelFormatter(IInputModel inputModel)
		{
			_properties = inputModel
							.GetType()
							.GetProperties()
							.Where(pi => pi.CanWrite)
							.Select(pi => new Properties
								{
									Name = pi.Name,
									TypeName = pi.PropertyType.Name,
									Value = pi.GetValue(inputModel, null).ToString()
								})
							.ToList();
		}

		public InputModelFormatter(ITypeMetadata metadata)
		{
			_properties = metadata.Properties.Select(pi => new Properties
			                                               	{
			                                               		Name = pi.Name,
			                                               		TypeName = pi.PropertyType.Name,
			                                               		Value = null
			                                               	}).ToList();
		}

		protected override string GetAsXml()
		{
			var root = new XElement("InputModel");

			foreach (var pi in _properties)
			{
				root.Add(
					new XElement(
						"Property", new XElement("PropertyName", pi.Name), new XElement("PropertyType", pi.TypeName)));
			}

			return root.ToString();
		}

		protected override object GetJsonObject(JsonSerializer serializer)
		{
			return _properties;
		}

		private class Properties
		{
			internal string Name { get; set; }
			internal string TypeName { get; set; }
			internal string Value { get; set; }
		}
	}
}