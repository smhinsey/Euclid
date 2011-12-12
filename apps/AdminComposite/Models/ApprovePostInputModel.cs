using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class ApprovePostInputModel : DefaultInputModel
	{
		public ApprovePostInputModel()
		{
			CommandType = typeof (ApprovePost);
		}

		public Guid PostIdentifier { get; set; }
		public Guid ApprovedBy { get; set; }
		public Guid CreatedBy { get; set; }
	}
}