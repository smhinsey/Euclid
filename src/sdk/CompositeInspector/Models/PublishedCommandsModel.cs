using System.Collections.Generic;
using Euclid.Common.Messaging;

namespace CompositeInspector.Models
{
	public class PublishedCommandsModel
	{
		public int CurrentPage { get; set; }

		public IEnumerable<IPublicationRecord> Records { get; set; }

		public int RowsPerPage { get; set; }
	}
}