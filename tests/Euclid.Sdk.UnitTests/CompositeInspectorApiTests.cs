using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompositeInspector;
using CompositeInspector.Extensions;
using CompositeInspector.Module;
using Euclid.Common.Extensions;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Euclid.TestingSupport;
using FakeItEasy;
using Nancy;
using Nancy.Responses;
using NUnit.Framework;

namespace Euclid.Sdk.UnitTests
{
	[Category(TestCategories.Unit)]
	public class CompositeInspectorApiTests
	{
		private ICompositeApp _compositeApp;
		private ICommandRegistry _registry;
		private IPublisher _publisher;
		private ICommand _testCommand;
		private ICommandPublicationRecord _publicationRecord;
		private IResponseFormatter _formatter;
		private ApiModule _euclidApi;
		private DefaultJsonSerializer _jsonSerializer;
		private DefaultXmlSerializer _xmlSerializer;
		
		private readonly IEnumerable<string> _acceptHtmlHeaders = new[] {"text/html"};
		private readonly IEnumerable<string> _acceptJsonHeaders = new[] {"application/json"};
		private readonly IEnumerable<string> _acceptXmlHeaders = new[] { "text/xml" };
		private readonly Guid _publicationId = new Guid("0489bc65-f1fc-4dbd-bcd2-711287a7b7c9");
		[SetUp]
		public void Setup()
		{
			
			_testCommand = A.Fake<ICommand>();
			_publisher = A.Fake<IPublisher>();
			_compositeApp = A.Fake<ICompositeApp>();
			_registry = A.Fake<ICommandRegistry>();
			_formatter = A.Fake<IResponseFormatter>();
			_publicationRecord = A.Fake<ICommandPublicationRecord>();
			_jsonSerializer = new DefaultJsonSerializer();
			_xmlSerializer = new DefaultXmlSerializer();

			A.CallTo(() => _testCommand.Created).Returns(DateTime.MaxValue);
			A.CallTo(() => _testCommand.CreatedBy).Returns(new Guid("ba5f18dc-e287-4d9e-ae71-c6989b10d778"));
			A.CallTo(() => _testCommand.Identifier).Returns(new Guid("ba5f18dc-e287-4d9e-ae71-c6989b10d778"));
			A.CallTo(() => _formatter.Serializers).Returns(new List<ISerializer> { _jsonSerializer, _xmlSerializer });
			A.CallTo(() => _publicationRecord.Dispatched).Returns(true);
			A.CallTo(() => _publicationRecord.Error).Returns(false);
			A.CallTo(() => _publicationRecord.Completed).Returns(true);
			A.CallTo(() => _publicationRecord.Created).Returns(DateTime.MinValue);
			A.CallTo(() => _publicationRecord.MessageLocation).Returns(new Uri("http://localhost/fake/message"));
			A.CallTo(() => _publicationRecord.MessageType).Returns(typeof(IPublicationRecord));
			A.CallTo(() => _publicationRecord.CreatedBy).Returns(Guid.Empty);
			A.CallTo(() => _compositeApp.GetCommandForInputModel(A.Dummy<IInputModel>())).Returns(_testCommand);
			A.CallTo(() => _publisher.PublishMessage(A.Fake<ICommand>())).Returns(_publicationId);
			A.CallTo(() => _registry.GetPublicationRecord(_publicationId)).Returns(_publicationRecord);

			_euclidApi = new ApiModule(_compositeApp, _registry, _publisher);
		}

		private void configureResponse(ResponseFormat expectedFormat)
		{
			Request req;
			var url = new Url {Scheme="http", Path="/"};
			switch(expectedFormat)
			{
				case ResponseFormat.Html:
					req = new Request("GET", url, null, new Dictionary<string, IEnumerable<string>>{ {"Accept", _acceptHtmlHeaders} }, null);
					break;
				case ResponseFormat.Json:
					req = new Request("GET", url, null, new Dictionary<string, IEnumerable<string>> { { "Accept", _acceptJsonHeaders } }, null);
					break;
				case ResponseFormat.Xml:
					req = new Request("GET", url, null, new Dictionary<string, IEnumerable<string>> { { "Accept", _acceptXmlHeaders } }, null);
					break;
				default:
					Assert.Fail("Invalid response format");
					return;
			}

			_euclidApi.Context = new NancyContext {Request = req};
			_euclidApi.Response = _formatter;
			A.CallTo(() => _formatter.Context).Returns(_euclidApi.Context);
		}

		[Test]
		public void Response_format_can_be_determined_by_accept_headers()
		{
			foreach (var format in Enum.GetValues(typeof(ResponseFormat)).Cast<ResponseFormat>())
			{
				configureResponse(format);
				Assert.AreEqual(format, _euclidApi.GetResponseFormat());
			}
		}

		[Test]
		public void Exceptions_are_returned_in_the_requested_format()
		{
			const string expectedJson = 
@"{
	""name"":""InvalidOperationException"",
	""message"":""FakeException"",
	""callStack"":null
}";

			const string expectedXml = 
@"<?xml version=""1.0""?>
<Exception xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
	<name>InvalidOperationException</name>
	<message>FakeException</message>
</Exception>";

			var ex = new InvalidOperationException("FakeException", null);

			foreach (var format in Enum.GetValues(typeof(ResponseFormat)).Cast<ResponseFormat>())
			{
				configureResponse(format);
				switch (format)
				{
					case ResponseFormat.Html:
						Assert.IsNull(ex.CreateResponse(format, _formatter));
						break;
					case ResponseFormat.Xml:
						var xml = ex.CreateResponse(format, _formatter);
						assertAreEqual(xml, expectedXml);
						break;
					case ResponseFormat.Json:
						var json = ex.CreateResponse(format, _formatter);
						assertAreEqual(json, expectedJson);
						break;
				}
			}
		}

		private static void assertAreEqual(Response response, string expected)
		{
			var streamAction = response.Contents;
			using (var s = new MemoryStream())
			{
				streamAction.Invoke(s);
				var value = s.GetString(Encoding.UTF8);

				Console.WriteLine(expected);
				value = Regex.Replace(value, @"\s", "");
				expected = Regex.Replace(expected, @"\s", "");
				Assert.True(expected.Equals(value, StringComparison.OrdinalIgnoreCase), string.Format("{1}expected: {0}{1}  actual: {2}", expected, Environment.NewLine, value));
			}
		}

		[Test]
		public void Publish_returns_formatted_publication_record()
		{
			foreach (var format in Enum.GetValues(typeof(ResponseFormat)).Cast<ResponseFormat>())
			{
				string contentType = string.Empty;
				switch (format)
				{
					case ResponseFormat.Xml:
						contentType = "application/xml";
						break;
					case ResponseFormat.Json:
						contentType = "application/json";
						break;
					default:
						continue;
				}

				configureResponse(format);
				var response = _euclidApi.PublishCommand(A.Dummy<IInputModel>());
				
				Assert.IsNotNull(response);
				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				Assert.AreEqual(contentType, response.ContentType);
			}

			A.CallTo(() => _publisher.PublishMessage(null)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Twice);
			A.CallTo(() => _registry.GetPublicationRecord(_publicationId)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Twice);
		}

		[Test]
		public void Get_command_metadata_returns_formatted_itypemetadata()
		{
			
		}

		[Test]
		public void Get_inputmodel_metadata_returns_formatted_itypemetadata()
		{
			
		}
	}
}
