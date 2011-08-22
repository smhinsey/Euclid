using System;
using Euclid.Composites.Mvc.Models;

namespace MetadataComposite.Areas.Metadata.Models
{
	public class InputModelFakeCommand4 : InputModelBase
	{
		public DateTime BirthDay { get; set; }

		public string Password { get; set; }
	}
}