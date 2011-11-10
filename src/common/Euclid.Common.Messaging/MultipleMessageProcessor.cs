namespace Euclid.Common.Messaging
{
	public abstract class MultipleMessageProcessor : IMessageProcessor
	{
		public bool CanProcessMessage(IMessage message)
		{
			var currentType = GetType();

			var methods = currentType.GetMethods();

			foreach (var method in methods)
			{
				var parameters = method.GetParameters();

				if (parameters.Length == 1 && parameters[0].ParameterType == message.GetType())
				{
					return true;
				}
			}

			return false;
		}
	}
}