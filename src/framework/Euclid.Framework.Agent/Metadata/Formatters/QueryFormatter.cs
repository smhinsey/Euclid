
d:\Projects\Euclid\platform>@git.exe %*
using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.Agent.Extensions;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata.Formatters
{
    internal class QueryFormatter : MetadataFormatter
    {
        private readonly ITypeMetadata _partMetadata;
        private readonly IAgentMetadata _agentMetadata;

        public QueryFormatter(ITypeMetadata partMetadata)
        {
            _partMetadata = partMetadata;
            _agentMetadata = partMetadata.Type.Assembly.GetAgentMetadata();
        }

        protected override object GetJsonObject(JsonSerializer serializer)
        {
            return new {
                            AgentSystemName = _agentMetadata.SystemName,
                            _partMetadata.Namespace,
                            _partMetadata.Name,
                           Methods = _partMetadata.Methods.Select(method =>
                                                        new
                                                            {
                                                                Arguments = method.Arguments
                                                            .OrderBy(a => a.Order)
                                                            .Select(a => new
                                                                             {
                                                                                 ArgumentType = a.PropertyType.Name,
                                                                                 ArgumentName = a.Name
                                                                             }
                                                            ),
                                                                ReturnType = GetFormattedReturnType(method),
                                                                method.Name
                                                            })
                       };
        }

        protected override string GetAsXml()
        {
            var root = new XElement("Query",
                                   new XElement("AgentSystemName", _agentMetadata.SystemName), 
                                   new XElement("Namespace", _partMetadata.Namespace),
                                   new XElement("Name", _partMetadata.Name));

            var methods = new XElement("Methods");
            root.Add(methods);

            foreach(var method in _partMetadata.Methods)
            {
                var m = new XElement("Method",
                                     new XElement("ReturnType", GetFormattedReturnType(method)),
                                     new XElement("Name", method.Name));

                var args = new XElement("Arguments");
                foreach(var arg in method.Arguments.OrderBy(a=>a.Order))
                {
                    args.Add(new XElement("Argument",
                                          new XElement("ArgumentType", arg.PropertyType.Name),
                                          new XElement("ArgumentName", arg.Name)));
                
                }

                m.Add(args);
                methods.Add(m);
            }

            return root.ToString();
        }

        private static string GetFormattedReturnType(IMethodMetadata methodMetadata)
        {
            return (methodMetadata.ReturnType.Namespace != null && !methodMetadata.ReturnType.Namespace.Contains("Collection"))
                       ? methodMetadata.ReturnType.Name
                       : string.Format("List<{0}>", methodMetadata.ReturnType.GetGenericArguments()[0].Name);
        }
    }
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
