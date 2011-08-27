using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Converters;
using Euclid.Sdk.TestComposite.Models;
using Euclid.TestingSupport;
using Moq;
using NUnit.Framework;

namespace Euclid.Composite.InputModelMapping
{
    [TestFixture]
    [Category(TestCategories.Unit)]
    public class TransformerRegistryTests
    {
        private IInputModelTransformerRegistry _registry;

        private string _commandName;

        private IValueProvider _valueProvider;

        private Mock<ActionExecutingContext> _filterContext;

        private Mock<IPublisher> _publisher;

        private Dictionary<string, object> _actionParameters;
            
        [SetUp]
        public void Setup()
        {
            _commandName = typeof (TestCommand).Name; 

            _registry = new InputModelToCommandTransformerRegistry();

            _registry.Add(_commandName, new TestInputModelToCommandConverter());

            var nvc = new NameValueCollection() { { "Number", "7" } };

            _valueProvider = new NameValueCollectionValueProvider(nvc, CultureInfo.CurrentCulture);

            _actionParameters = new Dictionary<string, object>();

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("POST");
            request.SetupGet(r => r.Form).Returns(new NameValueCollection() {{"partName", _commandName}});
            request.SetupGet(r => r.Params).Returns(nvc);

            var httpContext = new Mock<HttpContextBase>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            
            _filterContext = new Mock<ActionExecutingContext>();
            _filterContext.SetupGet(c => c.HttpContext).Returns(httpContext.Object);
            _filterContext.SetupGet(c=>c.ActionParameters).Returns(_actionParameters);
            
            _publisher = new Mock<IPublisher>();
            _publisher.Setup(c => c.PublishMessage(It.IsAny<ICommand>())).Returns(Guid.NewGuid());
        }

        [Test]
        public void TestTransformerExtensionMethods()
        {
            var inputModel = _registry.GetInputModel(_commandName, _valueProvider);

            Assert.NotNull(inputModel);

            var testInputModel = inputModel as TestInputModel;

            Assert.NotNull(testInputModel);

            Assert.AreEqual(7, testInputModel.Number);
        }

        [Test]
        public void TestCommandPublisherActionFilter()
        {
            var commandPublisher = new CommandPublisherAttribute(_registry, _publisher.Object);

            commandPublisher.OnActionExecuting(_filterContext.Object);

            Assert.NotNull(_filterContext.Object.ActionParameters);

            Assert.True(_filterContext.Object.ActionParameters.ContainsKey("publicationId"));

            var rawPubId = _filterContext.Object.ActionParameters["publicationId"];

            Assert.NotNull(rawPubId);

            var pubId = Guid.Parse(rawPubId.ToString());

            Assert.AreNotEqual(Guid.Empty, pubId);
        }

    }
}