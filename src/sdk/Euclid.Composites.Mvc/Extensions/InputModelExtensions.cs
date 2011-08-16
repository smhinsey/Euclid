using System.Linq;
using System.Xml.Linq;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Models;
using Newtonsoft.Json;

namespace Euclid.Composites.Mvc.Extensions
{
    public static class InputModelExtensions
    {
        private class InputModelMetadataFormatter : MetadataFormatter
        {
            private readonly IInputModel _inputModel;

            public InputModelMetadataFormatter(IInputModel inputModel)
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

        public static IFormattedMetadataProvider GetMetadataFormatter(this IInputModel inputModel)
        {
            return new InputModelMetadataFormatter(inputModel);
        }
    }
}