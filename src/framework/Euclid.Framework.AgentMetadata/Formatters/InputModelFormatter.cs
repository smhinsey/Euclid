using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public class InputModelFormatter : MetadataFormatter
	{
		private readonly IList<Property> _properties;
		public InputModelFormatter(IInputModel inputModel)
		{
			_properties = inputModel
							.GetType()
							.GetProperties()
							.Where(pi => pi.CanWrite)
							.Select(pi =>
							        	{
							        		var v = pi.GetValue(inputModel, null);
											return new Property
							        		       	{
							        		       		Name = pi.Name,
							        		       		Type = pi.PropertyType.Name,
							        		       		Value = (v == null) ? string.Empty : pi.GetValue(inputModel, null).ToString(),
														Choices = pi.PropertyType.IsEnum ? Enum.GetNames(pi.PropertyType) : null,
														MultiChoice = pi.PropertyType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0
							        		       	};
							        	})
							.ToList();
		}

		public InputModelFormatter(ITypeMetadata metadata)
		{
			_properties = metadata.Properties.Select(pi => new Property
			                                               	{
			                                               		Name = pi.Name,
			                                               		Type = pi.PropertyType.Name,
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
						"Property", new XElement("Name", pi.Name), new XElement("Type", pi.Type)));
			}

			return root.ToString();
		}

		protected override object GetJsonObject(JsonSerializer serializer)
		{
			return new
					{
						Properties = _properties.Select(p => new {p.Name, p.Type, p.Value, p.Choices, p.MultiChoice})
					};
		}

		private class Property
		{
			internal string Name { get; set; }
			internal string Type { get; set; }
			internal string Value { get; set; }
			internal string[] Choices { get; set; }
			internal bool MultiChoice { get; set; }
		}
	}
}