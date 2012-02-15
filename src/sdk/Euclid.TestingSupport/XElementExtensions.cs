using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Euclid.TestingSupport
{
	public static class XElementExtensions
	{
		public static void AssertAttributeValue(this XElement element, string name, string value)
		{
			Assert.NotNull(element.Attribute(name));
			Assert.True(element.Attribute(name).Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
		}

		public static XElement GetElementById(this XElement root, string id)
		{
			var element = root.Descendants().FirstOrDefault(x => x.Attribute("id") != null && x.Attribute("id").Value.Equals(id, StringComparison.CurrentCultureIgnoreCase));
			Assert.NotNull(element, string.Format("No element exists with id {0}", id));

			return element;
		}
	}
}