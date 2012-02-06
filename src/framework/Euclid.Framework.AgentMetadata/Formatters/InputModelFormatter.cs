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
		private readonly string _commandName;
		private readonly string _modelName;
		public InputModelFormatter(IInputModel inputModel)
		{
			_modelName = inputModel.GetType().Name;
			_commandName = inputModel.CommandType.Name;
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
			var model = Activator.CreateInstance(metadata.Type);
			var propInfo = metadata.Type.GetProperties().Where(x => x.Name == "CommandType").FirstOrDefault();
			_commandName = ((Type)propInfo.GetValue(model, null)).Name;
			_modelName = metadata.Type.Name;
			_properties = metadata.Properties.Select(pi => new Property
			                                               	{
			                                               		Name = pi.Name,
			                                               		Type = pi.PropertyType.Name,
																Value = pi.PropertyType.GetDefaultValue(),
																Choices = pi.PropertyType.IsEnum ? Enum.GetNames(pi.PropertyType) : null,
																MultiChoice = pi.PropertyType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0
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
						CommandName = _commandName,
						ModelName = _modelName,
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