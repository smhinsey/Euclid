using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	public interface ICommandRegistry : IPublicationRegistry<ICommandPublicationRecord, ICommandPublicationRecord>
	{
	}
}