using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class DeleteStopWordInputModel : DefaultInputModel
	{
		public DeleteStopWordInputModel()
		{
			CommandType = typeof (DeleteStopWord);
		}

		public Guid StopWordIdentifier { get; set; }
	}
}