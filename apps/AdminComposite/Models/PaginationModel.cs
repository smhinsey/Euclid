using System;

namespace AdminComposite.Models
{
	public class PaginationModel
	{
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public int Offset { get; set; }
		public int PageSize { get; set; }
		public int TotalPosts { get; set; }
		public bool WriteTable { get; set; }
		public bool WriteTFoot { get; set; }
		public Guid ForumIdentifier { get; set; }

		public int CurrentPage
		{
			get { return Offset/PageSize + 1; }
		}

		public int TotalPages
		{
			get { return TotalPosts/PageSize; }
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
			get { return Offset + PageSize > TotalPosts - 1 ? TotalPosts - PageSize : Offset + PageSize; }
		}

		public int LastPageOffset
		{
			get { return TotalPosts - PageSize; }
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