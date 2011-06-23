using Euclid.Common.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Framework.Cqrs
{
    public class CommandDispatcher : MultitaskingMessageDispatcher<ICommandRegistry>
    {
        public CommandDispatcher(IServiceLocator container, ICommandRegistry publicationRegistry) : base(container, publicationRegistry)
        {
        }
    }
}
