using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class ActivateStopWordInputModel : DefaultInputModel
	{
		public ActivateStopWordInputModel()
		{
			CommandType = typeof (ActivateStopWord);
		}

		public Guid StopWordIdentifier { get; set; }
		public bool Active { get; set; }
	}
}