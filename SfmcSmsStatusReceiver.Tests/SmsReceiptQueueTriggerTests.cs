using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvenna.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using SfmcSmsStatusReceiver.Services;
using Moq.Contrib.HttpClient;
using Microsoft.Azure.Functions.Worker;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SfmcSmsStatusReceiver.Data;

namespace SfmcSmsStatusReceiver.Tests
{
    [TestClass]
    public class SmsReceiptQueueTriggerTests
    {
        private SmsReceiptQueueTrigger _trigger;
        private readonly ILogger<FunctionLog> _logger = new NullLogger<FunctionLog>();
        private readonly Mock<HttpMessageHandler> _handler;
        private IHttpClientFactory _httpClientFactory;

        public SmsReceiptQueueTriggerTests()
        {
            _handler = new Mock<HttpMessageHandler>();
        }

        [TestInitialize]
        public void Setup()
        {
            _httpClientFactory = _handler.CreateClientFactory();
            
        }

        [TestMethod]
        public async Task RunTest_ContentTypeInvalid()
        {
            //setup
            ServiceBusReceivedMessage msg = ServiceBusModelFactory.ServiceBusReceivedMessage(
                contentType: "text/plain", messageId: "123456", body: new BinaryData("hello message"));

            var mockAuth = new Mock<IAuth>();
            mockAuth.Setup(x => x.Initialize).Returns(Task.CompletedTask);
            mockAuth.Setup(x=>x.RequireAuth())
                .Returns(true);
            _trigger = new SmsReceiptQueueTrigger(_logger, _httpClientFactory, mockAuth.Object);

            var msgBody = new BinaryData("sample text only data in payload");

            var mockBusAction = new Mock<ServiceBusMessageActions>();
            mockBusAction
                .Setup(x=>x.DeadLetterMessageAsync(msg, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _trigger = new SmsReceiptQueueTrigger(_logger, _httpClientFactory, mockAuth.Object);

            //act
            var result = await _trigger.Run(msg, mockBusAction.Object);

            //assert
            Assert.IsFalse(result, "Content-Type of 'application/json' expected for service bus message");
        }

        [TestMethod]
        public async Task RunTest_OK()
        {
            //setup
            var payload = JsonContent.Create(new
            {
                eventID = "SMccd712227fd351c813b83feb34eb0416",
                eventTimestamp = "2023-12-04T17:48:11.1129968Z",
                source = "SMSTesting",
                trackingID = "14236",
                from = "+18558406017",
                to = "+18166793684",
                deliveryStatus = "sent",
                deliveryMessage = "sent",
                messageReceivedTimestamp = string.Empty
            });

            var body = await payload.ReadAsStringAsync();

            var msgBody = new BinaryData(body);
            ServiceBusReceivedMessage msg = ServiceBusModelFactory.ServiceBusReceivedMessage(
                contentType: "application/json", messageId: "123456", body: msgBody
                );

            var mockBusAction = new Mock<ServiceBusMessageActions>();
            mockBusAction
                .Setup(x => x.DeadLetterMessageAsync(msg, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockBusAction
                .Setup(x=>x.CompleteMessageAsync(It.IsAny<ServiceBusReceivedMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var mockAuth = new Mock<IAuth>();
            mockAuth.Setup(x=>x.Initialize)
                .Returns(Task.CompletedTask);
            mockAuth.Setup(x => x.RequireAuth())
                .Returns(true);

            var mockLoader = new Mock<IDataLoader>();
            mockLoader.Setup(x => x.UpsertDataAsync(It.IsAny<IAuth>(), It.IsAny<SfmcDataExtension>()))
                .ReturnsAsync(true);

            _trigger = new SmsReceiptQueueTrigger(_logger, _httpClientFactory, mockAuth.Object);
            _trigger.DataLoader = mockLoader.Object;

            //act
            var result = await _trigger.Run(msg, mockBusAction.Object);

            //assert
            Assert.IsTrue(result);

        }


    }
}
