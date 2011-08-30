namespace Euclid.Common.Logging
{
	/// <summary>
	/// 	A marker interface used as an attachment point for extension methods related to logging.
	/// </summary>
	public interface ILoggingSource
	{
		string Name { get; }
	}

	public class DefaultLoggingSource : ILoggingSource
	{
		public string Name
		{
			get { return GetType().Name; }
		}
	}
}