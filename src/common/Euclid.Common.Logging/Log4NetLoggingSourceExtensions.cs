using System;
using log4net;

namespace Euclid.Common.Logging
{
	public static class Log4NetLoggingSourceExtensions
	{
		public static void WriteDebugMessage(this ILoggingSource source, string message)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsDebugEnabled)
			{
				logger.Debug(message);
			}
		}

		public static void WriteErrorMessage(this ILoggingSource source, string message, Exception exception, params object[] formatParameters)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsErrorEnabled)
			{
				logger.Error(string.Format(message, formatParameters), exception);
			}
		}

		public static void WriteFatalMessage(this ILoggingSource source, string message, Exception exception)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsFatalEnabled)
			{
				logger.Fatal(message, exception);
			}
		}

		public static void WriteInfoMessage(this ILoggingSource source, string message, params object[] formatParameters)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsInfoEnabled)
			{
				logger.Info(string.Format(message, formatParameters));
			}
		}

		public static void WriteWarnMessage(this ILoggingSource source, string message)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsWarnEnabled)
			{
				logger.Warn(message);
			}
		}
	}
}