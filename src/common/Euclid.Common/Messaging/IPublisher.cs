using System;

namespace Euclid.Common.Messaging
{
	public interface IPublisher
	{
		Guid PublishMessage(IMessage message);
	}
}