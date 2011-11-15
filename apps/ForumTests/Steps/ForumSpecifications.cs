using System;
using Euclid.TestingSupport;
using ForumAgent.Queries;

namespace ForumTests.Steps
{
	public class ForumSpecifications : DefaultAgentSteps
	{
		public ForumSpecifications()
		{
			Initialize();
		}

		protected override Type TypeFromAgent
		{
			get
			{
				return typeof(PostQueries);
			}
		}
	}
}