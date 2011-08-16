using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Agent.Metadata.Formatters;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Composites.Mvc.Extensions
{
    public static class InputModelExtensions
    {
        private class InputModelMetadataFormatterFormatter : MetadataFormatterFormatter
        {
            private readonly IInputModel _inputModel;

            public InputModelMetadataFormatterFormatter(IInputModel inputModel)
            {
                _inputModel = inputModel;
            }

            public override object GetJsonObject(JsonSerializer serializer)
            {
                return _inputModel.GetType().GetProperties().Select(pi => new
                                                                              {
                                                                                  PropertyName = pi.Name,
                                                                                  PropertyType = pi.PropertyType.Name
                                                                              });
            }

            public override string GetAsXml()
            {
                var root = new XElement("InputModel");

                foreach(var pi in _inputModel.GetType().GetProperties())
                {
                    root.Add(new XElement("Property",
                                          new XElement("PropertyName", pi.Name),
                                          new XElement("PropertyType", pi.PropertyType.Name)));
                }

                return root.ToString();
            }
        }

        public static IMetadataFormatter GetMetadataFormatter(this IInputModel inputModel)
        {
            return new InputModelMetadataFormatterFormatter(inputModel);
        }
    }
}