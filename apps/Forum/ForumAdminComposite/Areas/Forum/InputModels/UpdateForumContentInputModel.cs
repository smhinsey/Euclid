using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class UpdateForumContentInputModel : DefaultInputModel
	{
		public  UpdateForumContentInputModel()
		{
			CommandType = typeof (UpdateForumContent);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid ContentIdentifier { get; set; }
		public bool Active { get; set; }
		public string Location { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public string PartialView { get; set; }
	}
}