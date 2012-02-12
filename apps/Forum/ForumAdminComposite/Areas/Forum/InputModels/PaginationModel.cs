using System;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class PaginationModel
	{
		private string _identifierName = "forumId";

		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public int Offset { get; set; }
		public int PageSize { get; set; }
		public int TotalItems { get; set; }
		public bool WriteTable { get; set; }
		public bool WriteTFoot { get; set; }
		public bool WriteTr { get; set; }
		public int ColSpan { get; set; }
		public Guid Identifier { get; set; }
		
		public string IdentifierParameterName
		{
			get { return _identifierName; }
			set { _identifierName = value; }
		}

		public int CurrentPage
		{
			get { return Offset/PageSize + 1; }
		}

		public int TotalPages
		{
			get { return TotalItems/PageSize; }
		}

		public bool ShowLeadingEllipsis
		{
			get { return CurrentPage > 3; }
		}

		public bool ShowTrailingEllipsis
		{
			get { return TotalPages - CurrentPage > 2; }
		}

		public int PreviousPageOffset
		{
			get { return Offset - PageSize < 0 ? 0 : Offset - PageSize; }
		}

		public int NextPageOffset
		{
			get { return Offset + PageSize > TotalItems - 1 ? TotalItems - PageSize : Offset + PageSize; }
		}

		public int LastPageOffset
		{
			get { return TotalItems - PageSize; }
		}

		public int FirstPageOffset
		{
			get { return 0; }
		}

		public int GetOffsetForPage(int fromCurrent)
		{
			if ((CurrentPage + fromCurrent < 0) || (CurrentPage + fromCurrent > TotalPages - 1))
			{
				return -1;
			}

			return Offset + (fromCurrent*PageSize);
		}
	}
}