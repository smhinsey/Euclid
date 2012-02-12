using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class PublishPostInputModel : DefaultInputModel
	{
		public PublishPostInputModel()
		{
			CommandType = typeof (PublishPost);
		}

		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid ForumIdentifier { get; set; }

		public Guid CategoryIdentifier { get; set; }

		public string Title { get; set; }
		
		public string[] Tags { get; set; }
	}
}