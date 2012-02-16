using System.Collections.Generic;
using Euclid.Common.Messaging;
using Euclid.Framework.Models;

namespace LoggingAgent.ReadModels
{
	public class PublicationRecords : SyntheticReadModel
	{
		public int Offset { get; set; }
		public int RecordsPerPage { get; set; }
		public int TotalRecords { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage;

		public IEnumerable<IPublicationRecord> Records { get; set; }
	}
}