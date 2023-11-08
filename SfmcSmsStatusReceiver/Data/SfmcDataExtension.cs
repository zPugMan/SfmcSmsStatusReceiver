using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Data
{
    public class SfmcDataExtension
    {
        public SfmcDataExtension() { }

        public SfmcDataExtension(QueueMessage? data) 
        {
            if(data== null) 
                throw new ArgumentNullException("Message data is empty");

            this.LoadData(data);
        }

        public void LoadData(QueueMessage data)
        {
            Key = new SfmcDataExtensionKey() { MobilePhone = data.ToPhone };
            Values = new SfmcDataExtensionValues()
            {
                MobilePhone = data.ToPhone,
                FromPhone = data.FromPhone,
                Message = String.Empty,
                TrackingID = data.TrackingId,
                DeliveryStatus = data.DeliveryStatus,
                EventId = data.EventId
            };
        }

        public bool IsValidForUpload()
        {
            if(Key == null) return false;
            if(Values == null) return false;
            if(String.IsNullOrEmpty(Key.MobilePhone)) return false;

            return true;
        }

        [JsonProperty("keys")]
        public SfmcDataExtensionKey Key { get; set; }

        [JsonProperty("values")]
        public SfmcDataExtensionValues Values { get; set; }
    }

    public class SfmcDataExtensionKey
    {
        [JsonProperty("mobilephone")]
        public string MobilePhone { get; set; }
    }

    public class SfmcDataExtensionValues
    {
        [JsonProperty("mobilephone")]
        public string MobilePhone { get; set; }

        [JsonProperty("fromphone")]
        public string FromPhone { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("trackingid")]
        public string TrackingID { get;set; }

        [JsonProperty("deliverystatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("twilioeventid")]
        public string EventId { get; set; }
    }
}
