using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Data
{
    public class QueueMessage
    {
        [JsonPropertyName("eventID")]
        public string EventId { get; set; }

        [JsonPropertyName("eventTimestamp")]
        public string EventTimestamp { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("trackingID")]
        public string TrackingId { get; set; }

        [JsonPropertyName("from")]
        public string FromPhone { get; set; }

        [JsonPropertyName("to")]
        public string ToPhone { get; set; }

        [JsonPropertyName("deliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonPropertyName("deliveryMessage")]
        public string DeliveryMessage { get; set; }

        [JsonPropertyName("messageReceivedTimestamp")]
        public string MessageReceivedTimestamp { get;set; }

    }
}
