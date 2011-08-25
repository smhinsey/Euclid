using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	The settings required by a MessageDispatcher in order to configure itself and
	/// 	begin dispatching messages.
	/// </summary>
	public interface IMessageDispatcherSettings : IOverridableSettings
	{
		/// <summary>
		/// 	Gets the duration of a dispatch slice. Dispatching is divided into slices of time during
		/// 	which only a certain number of messages can be dispatched.
		/// </summary>
		IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; }

		/// <summary>
		/// 	Gets the input channel from which incoming messages are recieved.
		/// </summary>
		IOverridableSetting<IMessageChannel> InputChannel { get; }

		/// <summary>
		/// 	Gets the invalid channel, where messages which cannot be dispatched are sent.
		/// </summary>
		IOverridableSetting<IMessageChannel> InvalidChannel { get; }

		/// <summary>
		/// 	Gets a list of the message processors the dispatcher can deliver messages to.
		/// </summary>
		IOverridableSettingList<Type> MessageProcessorTypes { get; }

		/// <summary>
		/// 	Gets the number of messages to dispatch per slice of time. Dispatching is divided into slices of time during
		/// 	which only a certain number of messages can be dispatched.
		/// </summary>
		IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; }
	}
}
