using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using ForumAgent.ReadModels;

namespace ForumTests
{
	public class AutoMapperConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return (member.Name == "Identifier");
		}

		public override bool ShouldMap(Type type)
		{
			return type == typeof (Category) ||
			       type == typeof (Comment) ||
			       type == typeof (Post) ||
			       type == typeof (User) ||
			       type == typeof (UserProfile);
		}
	}
}