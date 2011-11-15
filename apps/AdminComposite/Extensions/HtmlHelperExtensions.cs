using System;
using System.Web.Mvc;

namespace AdminComposite.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static bool AreControllerAndActionAreCurrent(
			this HtmlHelper helper, string expectedController, string expectedAction)
		{
			var controller = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
			var action = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

			return ((expectedAction == null && expectedController.Equals(controller, StringComparison.OrdinalIgnoreCase))
			        ||
			        (expectedAction != null && expectedAction.Equals(action, StringComparison.OrdinalIgnoreCase)
			         && expectedController.Equals(controller, StringComparison.OrdinalIgnoreCase)));
		}

		public static string GetClassWhenControllerAndActionAreCurrent(
			this HtmlHelper helper, bool additional, string expectedController, string expectedAction, string className)
		{
			return (additional && helper.AreControllerAndActionAreCurrent(expectedController, expectedAction))
			       	? string.Format(@"class={0}", className)
			       	: string.Empty;
		}

		public static string GetClassWhenControllerAndActionAreCurrent(
			this HtmlHelper helper, string expectedController, string expectedAction, string className)
		{
			return helper.GetClassWhenControllerAndActionAreCurrent(true, expectedController, expectedAction, className);
		}

		public static string GetClassWhenControllerIsCurrent(
			this HtmlHelper helper, string expectedController, string className)
		{
			return GetClassWhenControllerAndActionAreCurrent(helper, expectedController, null, className);
		}
	}
}