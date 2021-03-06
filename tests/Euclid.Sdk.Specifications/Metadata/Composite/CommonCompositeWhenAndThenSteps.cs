﻿using Euclid.Common.Configuration;
using Euclid.Common.Messaging;
using Euclid.Composites;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Composite
{
	[Binding]
	[StepScope(Feature = "Composite provides configuration metadata")]
	public class CommonCompositeWhenAndThenSteps : CompositeTestProperties
	{
		[Given("a composite that (.*) configured")]
		public void GetComposite(string isOrIsnt)
		{
			var settings = new CompositeAppSettings();
			if (isOrIsnt.ToLower() == "is")
			{
				settings.OutputChannel.ApplyOverride(typeof(InMemoryMessageChannel));
			}

			try
			{
				Composite = new BasicCompositeApp
					{
						Name = "Euclid.Sdk.Specifications.Metadata.Composite",
						Description = "A composite used to test the metadata system"
					};

				Composite.Configure(settings);
			}
			catch (NullSettingException)
			{
				if (isOrIsnt.ToLower() != "isn't")
				{
					Assert.Fail("the composite could not be configured");
				}
			}
			catch
			{
				Assert.Fail("the composite could not be configured");
			}

			Formatter = Composite.GetFormatter();
		}
	}
}