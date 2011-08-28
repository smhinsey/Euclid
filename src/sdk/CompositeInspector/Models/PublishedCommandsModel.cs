using System.Collections.Generic;
using System.Linq;
using Euclid.Common.Messaging;

namespace CompositeInspector.Models
{
	public class PublishedCommandsModel
	{
	    private int _recordCount = -1;

		public int CurrentPage { get; set; }

		public IEnumerable<IPublicationRecord> Records { get; set; }

		public int RowsPerPage { get; set; }

	    public int TotalPages
	    {
	        get { return RecordCount/RowsPerPage; }
	    }

        public int RecordCount
        {
            get
            {
                if (_recordCount < 0)
                {
                    RecordCount = Records.Count();
                }

                return _recordCount;
            }

            private set { _recordCount = value; }
        }

        public bool HasPreviousPage { get { return CurrentPage - 1 >= 0; } }

        public bool HasNextPage { get { return CurrentPage + 1 <= TotalPages; } }
	}
}