using System;
using System.Collections.Generic;
using System.Text;

namespace Euclid.Composites.Conversion
{
	public class CannotMapCommandException : Exception
	{
		private readonly string _commandName;

		private readonly IEnumerable<string> _inputModels;

		public CannotMapCommandException(string commandName, IEnumerable<string> inputModels)
		{
			_commandName = commandName;
			_inputModels = inputModels;
		}

		public override string Message
		{
			get
			{
				var message = new StringBuilder();
				message.AppendFormat("Unable to find an input model for the command '{0}'", _commandName);
				message.AppendLine();
				message.AppendLine();

				if (string.IsNullOrEmpty(_commandName))
				{
					message.Append("The CommandName is blank, is the command type specified in the input model definition?");
					message.AppendLine();
					message.AppendLine();
				}

				message.AppendFormat("The following input models are registered with the composite:");
				message.AppendLine();
				foreach (var inputModel in _inputModels)
				{
					message.AppendFormat("\t{0}", inputModel);
					message.AppendLine();
				}

				return message.ToString();
			}
		}
	}
}