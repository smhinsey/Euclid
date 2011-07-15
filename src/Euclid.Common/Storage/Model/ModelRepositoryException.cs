using System;

namespace Euclid.Common.Storage.NHibernate
{
	public class ModelRepositoryException : Exception
	{
		public ModelRepositoryException(string message) : base(message)
		{
		}
	}
}