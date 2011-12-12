using System;
using Euclid.Composites.Mvc.Models;

namespace AdminComposite.Models
{
	public class RejectPostInputModel : DefaultInputModel
	{
		public Guid CreatedBy { get; set; }
		public Guid PostIdentifier { get; set; }
	}
}