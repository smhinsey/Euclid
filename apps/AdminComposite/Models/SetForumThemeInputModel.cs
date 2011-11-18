using System;
using System.Collections.Generic;
using Euclid.Composites.Mvc.Models;

namespace AdminComposite.Models
{
	public class SetForumThemeInputModel : DefaultInputModel
	{
		public Guid ForumIdentifier { get; set; }
		public Tuple<string, string> CurrentTheme { get; set; }

		public IList<Tuple<string, string>> AvailableThemes { get; set; }
	}
}