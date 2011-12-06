using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class InputModelExtensions
	{
		public static IMetadataFormatter GetMetadataFormatter(this IInputModel inputModel)
		{
			return new InputModelFormatter(inputModel);
		}
	}
}