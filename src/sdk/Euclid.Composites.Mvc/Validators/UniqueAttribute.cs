using System.ComponentModel.DataAnnotations;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Framework.Models;
using ForumAgent.Queries;

namespace Euclid.Composites.Mvc.Validators
{
	public class UniqueAttribute : ValidationAttribute
	{
		public UniqueAttribute()
		{
			return new UniqueAttributeHelper<AvatarQueries>();
		}
	}
	
	internal class UniqueAttributeHelper<TReadModel> 
		where TReadModel: NhQuery<IReadModel>
	{
		
	}
}