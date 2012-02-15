using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Xml.Linq;
using Euclid.Composites.Mvc;
using Euclid.Sdk.TestComposite.Models;
using Euclid.TestingSupport;
using FakeItEasy;
using NUnit.Framework;

namespace Euclid.Sdk.UnitTests
{
	public class HtmlHelperFixture
	{
		[Test]
		public void HtmlHelper_renders_correct_action_for_publishing_input_model()
		{
			var inputModel = new ComplexInputModel();
			const string currentPage = "fake-page?name=value&foo=bar";
			withInputModelForm(
				currentPage,
				helper => helper.BeginFormForInputModel(inputModel),
				formElement =>
					{
						formElement.AssertAttributeValue("action", "/composite/api/publish");
						formElement.AssertAttributeValue("method", "post");
						formElement.AssertAttributeValue("encType", "multipart/form-data");

						var redirectElement = formElement.GetElementById("inputmodel-redirecturl");
						redirectElement.AssertAttributeValue("type", "hidden");
						redirectElement.AssertAttributeValue("name", "redirectUrl");
						redirectElement.AssertAttributeValue("value", currentPage);

						var agentNameElement = formElement.GetElementById("inputmodel-agentSystemName");
						agentNameElement.AssertAttributeValue("type", "hidden");
						agentNameElement.AssertAttributeValue("name", "agentSystemName");
						agentNameElement.AssertAttributeValue("value", inputModel.AgentSystemName);

						var partNameElement = formElement.GetElementById("inputmodel-partName");
						partNameElement.AssertAttributeValue("type", "hidden");
						partNameElement.AssertAttributeValue("name", "partName");
						partNameElement.AssertAttributeValue("value", inputModel.CommandType.Name);
					});
		}

		[Test]
		public void HtmlHelper_override_redirect_url()
		{
			var inputModel = new ComplexInputModel();
			const string currentPage = "fake-page?name=value&foo=bar";
			const string redirectTo = "new-page";

			withInputModelForm(
				currentPage,
				helper => helper.BeginFormForInputModel(inputModel, true, redirectTo),
				formElement =>
					{
						formElement.AssertAttributeValue("action", "/composite/api/publish");
						formElement.AssertAttributeValue("method", "post");
						formElement.AssertAttributeValue("encType", "multipart/form-data");

						var redirectElement = formElement.GetElementById("inputmodel-redirecturl");
						redirectElement.AssertAttributeValue("type", "hidden");
						redirectElement.AssertAttributeValue("name", "redirectUrl");
						redirectElement.AssertAttributeValue("value", redirectTo);

						var agentNameElement = formElement.GetElementById("inputmodel-agentSystemName");
						agentNameElement.AssertAttributeValue("type", "hidden");
						agentNameElement.AssertAttributeValue("name", "agentSystemName");
						agentNameElement.AssertAttributeValue("value", inputModel.AgentSystemName);

						var partNameElement = formElement.GetElementById("inputmodel-partName");
						partNameElement.AssertAttributeValue("type", "hidden");
						partNameElement.AssertAttributeValue("name", "partName");
						partNameElement.AssertAttributeValue("value", inputModel.CommandType.Name);
					}
				);
		}

		[Test]
		public void HtmlHelper_override_parameter_must_be_true_to_redirect()
		{
			var inputModel = new ComplexInputModel();
			const string currentPage = "fake-page?name=value&foo=bar";
			const string redirectTo = "new-page";

			withInputModelForm(
				currentPage,
				helper => helper.BeginFormForInputModel(inputModel, false, redirectTo),
				formElement =>
				{
					formElement.AssertAttributeValue("action", "/composite/api/publish");
					formElement.AssertAttributeValue("method", "post");
					formElement.AssertAttributeValue("encType", "multipart/form-data");

					var redirectElement = formElement.GetElementById("inputmodel-redirecturl");
					redirectElement.AssertAttributeValue("type", "hidden");
					redirectElement.AssertAttributeValue("name", "redirectUrl");
					redirectElement.AssertAttributeValue("value", currentPage);

					var agentNameElement = formElement.GetElementById("inputmodel-agentSystemName");
					agentNameElement.AssertAttributeValue("type", "hidden");
					agentNameElement.AssertAttributeValue("name", "agentSystemName");
					agentNameElement.AssertAttributeValue("value", inputModel.AgentSystemName);

					var partNameElement = formElement.GetElementById("inputmodel-partName");
					partNameElement.AssertAttributeValue("type", "hidden");
					partNameElement.AssertAttributeValue("name", "partName");
					partNameElement.AssertAttributeValue("value", inputModel.CommandType.Name);
				});
		}

		[Test]
		public void HtmlHelper_can_set_form_id()
		{
			var inputModel = new ComplexInputModel();
			const string currentPage = "fake-page?name=value&foo=bar";
			const string formId = "form-id";

			withInputModelForm(
				currentPage,
				helper => helper.BeginFormForInputModel(inputModel, false, string.Empty, formId),
				formElement =>
				{
					formElement.AssertAttributeValue("action", "/composite/api/publish");
					formElement.AssertAttributeValue("method", "post");
					formElement.AssertAttributeValue("encType", "multipart/form-data");
					formElement.AssertAttributeValue("id", formId);

					var redirectElement = formElement.GetElementById("inputmodel-redirecturl");
					redirectElement.AssertAttributeValue("type", "hidden");
					redirectElement.AssertAttributeValue("name", "redirectUrl");
					redirectElement.AssertAttributeValue("value", currentPage);

					var agentNameElement = formElement.GetElementById("inputmodel-agentSystemName");
					agentNameElement.AssertAttributeValue("type", "hidden");
					agentNameElement.AssertAttributeValue("name", "agentSystemName");
					agentNameElement.AssertAttributeValue("value", inputModel.AgentSystemName);

					var partNameElement = formElement.GetElementById("inputmodel-partName");
					partNameElement.AssertAttributeValue("type", "hidden");
					partNameElement.AssertAttributeValue("name", "partName");
					partNameElement.AssertAttributeValue("value", inputModel.CommandType.Name);
				});

		}

		private static void withInputModelForm(string currentPage, Func<HtmlHelper, MvcForm> getForm, Action<XElement> workWithFormHtml)
		{
			using (var writer = new StringWriter())
			{
				var viewCtx = A.Fake<ViewContext>();
				A.CallTo(() => viewCtx.Writer).Returns(writer);
				A.CallTo(() => viewCtx.RequestContext.HttpContext.Request.RawUrl).Returns(currentPage);

				var viewDataContainer = A.Fake<IViewDataContainer>();
				A.CallTo(() => viewDataContainer.ViewData).Returns(new ViewDataDictionary());

				var helper = new HtmlHelper(viewCtx, viewDataContainer);
				var mvcForm = getForm(helper);

				mvcForm.EndForm();
				Assert.NotNull(mvcForm);

				var html = writer.GetStringBuilder().ToString();
				Console.WriteLine(html);

				workWithFormHtml(XElement.Parse(html));
			}
		}
	}
}