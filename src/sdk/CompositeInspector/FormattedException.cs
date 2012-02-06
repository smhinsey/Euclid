using System.Xml.Serialization;

namespace CompositeInspector
{
	[XmlRoot("Exception")]
	public class FormattedException
	{
		public FormattedException()
		{
		}

// ReSharper disable InconsistentNaming
		public string name { get; set; }
		public string message { get; set; }
		public string callStack { get; set; }
// ReSharper restore InconsistentNaming
	}
}