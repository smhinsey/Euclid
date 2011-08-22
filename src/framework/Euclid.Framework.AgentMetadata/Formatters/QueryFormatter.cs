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
			this._partMetadata = partMetadata;
			this._agentMetadata = partMetadata.Type.Assembly.GetAgentMetadata();
		}

		protected override string GetAsXml()
		{
			var root = new XElement(
				"Query", 
				new XElement("AgentSystemName", this._agentMetadata.SystemName), 
				new XElement("Namespace", this._partMetadata.Namespace), 
				new XElement("Name", this._partMetadata.Name));

			var methods = new XElement("Methods");
			root.Add(methods);

			foreach (var method in this._partMetadata.Methods)
			{
				var m = new XElement(
					"Method", new XElement("ReturnType", GetFormattedReturnType(method)), new XElement("Name", method.Name));

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
						AgentSystemName = this._agentMetadata.SystemName, 
						this._partMetadata.Namespace, 
						this._partMetadata.Name, 
						Methods =
							this._partMetadata.Methods.Select(
								method =>
								new
									{
										Arguments =
									method.Arguments.OrderBy(a => a.Order).Select(
										a => new { ArgumentType = a.PropertyType.Name, ArgumentName = a.Name }), 
										ReturnType = GetFormattedReturnType(method), 
										method.Name
									})
					};
		}

		private static string GetFormattedReturnType(IMethodMetadata methodMetadata)
		{
			return (methodMetadata.ReturnType.Namespace != null && !methodMetadata.ReturnType.Namespace.Contains("Collection"))
			       	? methodMetadata.ReturnType.Name
			       	: string.Format("List<{0}>", methodMetadata.ReturnType.GetGenericArguments()[0].Name);
		}
	}
}