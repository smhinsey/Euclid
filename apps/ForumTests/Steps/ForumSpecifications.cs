using System;
using ForumAgent.Queries;

namespace ForumTests.Steps
{
	public class ForumSpecifications : AgentSpecificationTester
	{
		public ForumSpecifications()
		{
			Initialize();
		}

		protected override Type TypeFromAgent
		{
			get { return typeof (PostQueries); }
		}
	}
}