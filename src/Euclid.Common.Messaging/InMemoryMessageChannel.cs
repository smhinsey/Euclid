using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Euclid.Common.Messaging
{
	public class InMemoryMessageChannel : MessageChannelBase
	{
		//private static readonly ConcurrentQueue<IMessage> Queue = new ConcurrentQueue<IMessage>();
		private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
		private static readonly LinkedList<IMessage> MessageList = new LinkedList<IMessage>();

		public override void Clear()
		{
			TransportIsOpenFor("Clear");

			Lock.EnterWriteLock();

			try
			{
				MessageList.Clear();
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		public override ChannelState Close()
		{
			State = ChannelState.Closed;

			return State;
		}

		public override ChannelState Open()
		{
			State = ChannelState.Open;

			return State;
		}

		public override IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout)
		{
			TransportIsOpenFor("ReceiveMany");

			var start = DateTime.Now;

			var count = 0;

			Lock.EnterUpgradeableReadLock();

			try
			{
				while (MessageList.Count > 0 && DateTime.Now.Subtract(start) <= timeout && count < howMany)
				{
					var message = MessageList.First.Value;

					Lock.EnterWriteLock();

					try
					{
						MessageList.RemoveFirst();
					}
					finally
					{
						Lock.ExitWriteLock();
					}

					if (message == null) continue;

					count++;

					yield return message;
				}

				yield break;
			}
			finally
			{
				Lock.ExitUpgradeableReadLock();
			}
		}

		public override IEnumerable<TSubType> ReceiveMany<TSubType>(int howMany, TimeSpan timeSpan)
		{
			TransportIsOpenFor(string.Format("ReceiveMany<{0}>", typeof (TSubType).Name));

			var start = DateTime.Now;

			var count = 0;

			Lock.EnterWriteLock();

			try
			{
				while (MessageList.Count > 0 && DateTime.Now.Subtract(start) <= timeSpan && count < howMany)
				{
					var subTypeList = MessageList.OfType<TSubType>().ToList();

					if (subTypeList.Count() == 0)
					{
						break;
					}

					var message = subTypeList.First();

					MessageList.Remove(message);

					count++;

					yield return message;
				}

				yield break;
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}

		public override IMessage ReceiveSingle(TimeSpan timeout)
		{
			TransportIsOpenFor("ReceiveSingle");

			return ReceiveMany(1, timeout).First();
		}

		public override void Send(IMessage message)
		{
			TransportIsOpenFor("Send");

			Lock.EnterWriteLock();

			try
			{
				MessageList.AddLast(message);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
		}
	}
}