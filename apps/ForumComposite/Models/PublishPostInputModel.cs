using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class PublishPostInputModel : InputModelBase
	{
		public PublishPostInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(PublishPost);
		}

		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public string Title { get; set; }
	}
}