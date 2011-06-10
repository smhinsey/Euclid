namespace Euclid.Common.Configuration
{
	public abstract class ConfigContextBaseline
	{
		protected ConfigContextBaseline(ConfigurationContext currentContext)
		{
			CurrentContext = currentContext;
			AutoDetectContext = false;
		}

		protected ConfigContextBaseline(bool autoDetectContext)
		{
			AutoDetectContext = autoDetectContext;
		}

		public bool AutoDetectContext { get; private set; }
		public ConfigurationContext CurrentContext { get; private set; }
		public string FabricControllerInputConnectionString { get; set; }

		public void Initialize()
		{
			
		}
	}
}