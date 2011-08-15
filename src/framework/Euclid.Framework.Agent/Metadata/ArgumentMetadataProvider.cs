using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Euclid.Framework.Agent.Metadata
{
	public class ArgumentMetadataProvider : IArgumentMetadata
	{
		public object DefaultValue { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public Type PropertyType { get; set; }

        //public override string GetAsXml()
        //{
        //    return new XElement("ArgumentMetadata",
        //                        new XElement("ArgumentName", Name), 
        //                        new XElement("Type", PropertyType.FullName),
        //                        new XElement("DefaultValue", new XCData(DefaultValue.ToString())),
        //                        new XElement("Order", Order)
        //        ).ToString();
        //}
	}
}