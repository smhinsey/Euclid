using System;
using Euclid.Composites.Mvc.Models;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class ApproveCommentInputModel : DefaultInputModel
	{
		public Guid PostIdentifier { get; set; }
		public Guid ApprovedBy { get; set; }
		public Guid CreatedBy { get; set; }
	}
}