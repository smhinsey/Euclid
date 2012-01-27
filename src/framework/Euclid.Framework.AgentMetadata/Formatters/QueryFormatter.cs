using System;
using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.AgentMetadata.Extensions;
using Newtonsoft.Json;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	internal class QueryFormatter : MetadataFormatter
	{
		private readonly IAgentMetadata _agentMetadata;

		private readonly ITypeMetadata _partMetadata;

		public QueryFormatter(ITypeMetadata partMetadata)
		{
			_partMetadata = partMetadata;
			_agentMetadata = partMetadata.Type.Assembly.GetAgentMetadata();
		}

		protected override string GetAsXml()
		{
			var root = new XElement(
				"Query",
				new XElement("AgentSystemName", _agentMetadata.SystemName),
				new XElement("Namespace", _partMetadata.Namespace),
				new XElement("Name", _partMetadata.Name));

			var methods = new XElement("Methods");
			root.Add(methods);

			foreach (var method in _partMetadata.Methods)
			{
				var m = new XElement(
					"Method", new XElement("ReturnType", getFormattedReturnType(method)), new XElement("Name", method.Name));

				var args = new XElement("Arguments");
				foreach (var arg in method.Arguments.OrderBy(a => a.Order))
				{
					args.Add(
						new XElement(
							"Argument", new XElement("ArgumentType", arg.PropertyType.Name), new XElement("ArgumentName", arg.Name)));
				}

				m.Add(args);
				methods.Add(m);
			}

			return root.ToString();
		}

		protected override object GetJsonObject(JsonSerializer serializer)
		{
			return
				new
					{
						AgentSystemName = _agentMetadata.SystemName,
						_partMetadata.Namespace,
						_partMetadata.Name,
						Methods =
							_partMetadata.Methods.Select(
								method =>
								new
									{
										Arguments = method
														.Arguments
														.OrderBy(a => a.Order)
														.Select(a => 
															new
																{
																	ArgumentType = a.PropertyType.Name, 
																	ArgumentName = a.Name,
																	Choices = a.PropertyType.IsEnum ? Enum.GetNames(a.PropertyType) : null,
																	MultiChoice = a.PropertyType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0
																}),
										ReturnType = getFormattedReturnType(method), 
										method.Name
									})
					};
		}

		private static string getFormattedReturnType(IMethodMetadata methodMetadata)
		{
			return (methodMetadata.ReturnType.Namespace != null && !methodMetadata.ReturnType.Namespace.Contains("Collection"))
			       	? methodMetadata.ReturnType.Name
			       	: string.Format("List<{0}>", methodMetadata.ReturnType.GetGenericArguments()[0].Name);
		}
	}
}