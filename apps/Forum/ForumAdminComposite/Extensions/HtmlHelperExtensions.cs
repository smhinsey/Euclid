using System;
using System.Web.Mvc;

namespace AdminComposite.Extensions
{
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// returns true if the current route matches the speceified controller & action
		/// </summary>
		/// <param name="helper">html helper</param>
		/// <param name="expectedController">the name of the controller to check</param>
		/// <param name="expectedAction">the name of the action to check</param>
		/// <returns>boolean</returns>
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

		/// <summary>
		/// returns a class if the current route matches the specified controller and action, otherwise string.Empty
		/// </summary>
		/// <param name="helper">the html helper object</param>
		/// <param name="performCheck">whether to perform the check or not</param>
		/// <param name="expectedController">name of controller to check</param>
		/// <param name="expectedAction">name of action to check</param>
		/// <param name="className">class to return</param>
		/// <returns></returns>
		public static string GetClassWhenControllerAndActionAreCurrent(
			this HtmlHelper helper, bool performCheck, string expectedController, string expectedAction, string className)
		{
			return (performCheck && helper.AreControllerAndActionAreCurrent(expectedController, expectedAction))
			       	? string.Format(@"class={0}", className)
			       	: string.Empty;
		}

		/// <summary>
		/// returns a class if the current route matches the specified controller and action, otherwise string.Empty
		/// </summary>
		/// <param name="helper">the html helper object</param>
		/// <param name="expectedController">name of controller to check</param>
		/// <param name="expectedAction">name of action to check</param>
		/// <param name="className">class to return</param>
		/// <returns></returns>
		public static string GetClassWhenControllerAndActionAreCurrent(
			this HtmlHelper helper, string expectedController, string expectedAction, string className)
		{
			return helper.GetClassWhenControllerAndActionAreCurrent(true, expectedController, expectedAction, className);
		}

		/// <summary>
		/// returns a class if the current route matches the specified controller, otherwise string.Empty
		/// </summary>
		/// <param name="helper">the html helper object</param>
		/// <param name="expectedController">name of controller to check</param>
		/// <param name="className">class to return</param>
		/// <returns></returns>
		public static string GetClassWhenControllerIsCurrent(
			this HtmlHelper helper, string expectedController, string className)
		{
			return GetClassWhenControllerAndActionAreCurrent(helper, expectedController, null, className);
		}
	}
}