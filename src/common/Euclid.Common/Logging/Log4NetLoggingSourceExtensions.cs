using System;
using log4net;

namespace Euclid.Common.Logging
{
	/// <summary>
	/// 	Extension methods used by implementors of ILoggingSource to perform logging related operations.
	/// </summary>
	public static class Log4NetLoggingSourceExtensions
	{
		/// <summary>
		/// 	Writes a debug message to the logging stream.
		/// </summary>
		/// <param name = "source">An ILoggingSource implementation.</param>
		/// <param name = "message">The message to be written to the log.</param>
		/// <param name = "formatParameters">String formatting parameters.</param>
		public static void WriteDebugMessage(this ILoggingSource source, string message, params object[] formatParameters)
		{
			source.SetCustomLogFields();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsDebugEnabled)
			{
				logger.Debug(message);
			}
		}

		/// <summary>
		/// 	Writes an error message to the logging stream.
		/// </summary>
		/// <param name = "source">An ILoggingSource implementation.</param>
		/// <param name = "message">The message to be written to the log.</param>
		/// <param name = "exception">The exception associated with the log message.</param>
		/// <param name = "formatParameters">String formatting parameters.</param>
		public static void WriteErrorMessage(
			this ILoggingSource source, string message, Exception exception, params object[] formatParameters)
		{
			source.SetCustomLogFields();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsErrorEnabled)
			{
				logger.Error(string.Format(message, formatParameters), exception);
			}
		}

		// SELF all or nothing on the format parameters...

		/// <summary>
		/// 	Writes a fatal message to the logging stream.
		/// </summary>
		/// <param name = "source">An ILoggingSource implementation.</param>
		/// <param name = "message">The message to be written to the log.</param>
		/// <param name = "exception">The exception associated with the log message.</param>
		public static void WriteFatalMessage(this ILoggingSource source, string message, Exception exception)
		{
			source.SetCustomLogFields();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsFatalEnabled)
			{
				logger.Fatal(message, exception);
			}
		}

		/// <summary>
		/// 	Writes an info message to the logging stream.
		/// </summary>
		/// <param name = "source">An ILoggingSource implementation.</param>
		/// <param name = "message">The message to be written to the log.</param>
		/// <param name = "formatParameters">String formatting parameters.</param>
		public static void WriteInfoMessage(this ILoggingSource source, string message, params object[] formatParameters)
		{
			source.SetCustomLogFields();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsInfoEnabled)
			{
				logger.Info(string.Format(message, formatParameters));
			}
		}

		/// <summary>
		/// 	Writes a warning message to the logging stream.
		/// </summary>
		/// <param name = "source">An ILoggingSource implementation.</param>
		/// <param name = "message">The message to be written to the log.</param>
		public static void WriteWarnMessage(this ILoggingSource source, string message)
		{
			source.SetCustomLogFields();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsWarnEnabled)
			{
				logger.Warn(message);
			}
		}

		private static void SetCustomLogFields(this ILoggingSource source)
		{
			ThreadContext.Properties["Created"] = DateTime.Now;
			ThreadContext.Properties["Modified"] = DateTime.Now;
			ThreadContext.Properties["Id"] = 0;
		}
	}
}