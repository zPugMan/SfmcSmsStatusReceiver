using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvenna.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;

namespace SfmcSmsStatusReceiver.Tests
{
    [TestClass]
    public class SmsReceiptQueueTriggerTests
    {
        private readonly SmsReceiptQueueTrigger _trigger;
        private readonly ILogger<FunctionLog> _logger;
        

        public SmsReceiptQueueTriggerTests()
        {
            //using ILoggerFactory logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            //_logger = logFactory.CreateLogger<FunctionLog>();  //CreateLogger<FunctionLog>(nameof(SmsReceiptQueueTriggerTests));
            //_trigger = new SmsReceiptQueueTrigger(_logger);
        }

        [TestMethod]
        public async Task RunTest()
        {
            //setup
            ServiceBusReceivedMessage msg = ServiceBusModelFactory.ServiceBusReceivedMessage(
                contentType: "text/plain", messageId: "123456", body: new BinaryData("hello message"));

            var result = await _trigger.Run(msg, null);
            Assert.IsTrue(result);
        }
    }
}
