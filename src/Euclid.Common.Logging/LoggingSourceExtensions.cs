using System;
using log4net;

namespace Euclid.Common.Logging
{
	public static class LoggingSourceExtensions
	{
		public static void WriteInfoMessage(this ILoggingSource source, string message)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if(logger.IsInfoEnabled)
			{
				logger.Info(message);
			}
		}

		 public static void WriteDebugMessage(this ILoggingSource source, string message)
		 {
			 var logger = LogManager.GetLogger(source.GetType());

			 if (logger.IsDebugEnabled)
			 {
				 logger.Debug(message);
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

		 public static void WriteErrorMessage(this ILoggingSource source, string message, Exception exception)
		 {
			 var logger = LogManager.GetLogger(source.GetType());

			 if (logger.IsErrorEnabled)
			 {
				 logger.Error(message, exception);
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
	}
}