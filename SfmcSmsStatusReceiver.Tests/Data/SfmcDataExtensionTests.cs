using SfmcSmsStatusReceiver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Tests.Data
{
    [TestClass]
    public class SfmcDataExtensionTests
    {


        [TestMethod]
        public async Task LoadData_OK()
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

            var payloadJson = await payload.ReadAsStringAsync();
            var queueMsg = JsonSerializer.Deserialize<QueueMessage>(payloadJson);

            //act
            var dataExt = new SfmcDataExtension(queueMsg);

            //Assert
            Assert.IsTrue(queueMsg.EventId == "SMccd712227fd351c813b83feb34eb0416", $"Deserialization issue for {nameof(QueueMessage.EventId)}");
            Assert.IsTrue(dataExt.Values.EventId.Equals(queueMsg.EventId, StringComparison.CurrentCulture), $"DataExtension failed to load {nameof(QueueMessage.EventId)}");
        }

        [TestMethod]
        public async Task SeralizationTest()
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

            var payloadJson = await payload.ReadAsStringAsync();
            var queueMsg = JsonSerializer.Deserialize<QueueMessage>(payloadJson);

            //act
            var dataExt = new SfmcDataExtension(queueMsg);
            var jsonResult = JsonSerializer.Serialize<SfmcDataExtension>(dataExt);

            //assert
            Assert.IsTrue(jsonResult.Contains("twilioeventid", StringComparison.CurrentCulture), "JSON from DataExtension expecting 'twilioeventid'");


        }
    }
}
