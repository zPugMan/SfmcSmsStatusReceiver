using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SfmcSmsStatusReceiver.Data
{
    public class QueueMessage
    {
        [JsonProperty("eventID")]
        public string EventId { get; set; }

        [JsonProperty("eventTimestamp")]
        public string EventTimestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("trackingID")]
        public string TrackingId { get; set; }

        [JsonProperty("from")]
        public string FromPhone { get; set; }

        [JsonProperty("to")]
        public string ToPhone { get; set; }

        [JsonProperty("deliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("deliveryMessage")]
        public string DeliveryMessage { get; set; }

        [JsonProperty("messageReceivedTimestamp")]
        public string MessageReceivedTimestamp { get;set; }

    }
}
