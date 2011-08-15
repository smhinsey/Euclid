using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Euclid.Framework.Agent.Metadata
{
    internal class QueryFormatter : MetadataFormatter
    {
        private readonly TypeMetadata _partMetadata;

        public QueryFormatter(TypeMetadata partMetadata)
        {
            _partMetadata = partMetadata;
        }

        public override object GetJsonObject(JsonSerializer serializer)
        {
            return _partMetadata.Methods.Select(method =>
                                                new
                                                    {
                                                        Arguments = method.Arguments
                                                    .OrderBy(a => a.Order)
                                                    .Select(a => new {
                                                                         ArgumentType = a.PropertyType.Name,
                                                                         ArgumentName = a.Name}
                                                    ),
                                                        ReturnType = GetFormattedReturnType(method),
                                                        method.Name
                                                    });
        }

        public override string GetAsXml()
        {
            var root = new XElement("Query");

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