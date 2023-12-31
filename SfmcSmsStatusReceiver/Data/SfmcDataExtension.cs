﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Data
{
    public class SfmcDataExtension : IDataObject
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

        [JsonPropertyName("keys")]
        public SfmcDataExtensionKey Key { get; set; }

        [JsonPropertyName("values")]
        public SfmcDataExtensionValues Values { get; set; }
    }

    public class SfmcDataExtensionKey
    {
        private string mobilePhone;
        [JsonPropertyName("mobilephone")]
        public string MobilePhone {
            get { return mobilePhone; }
            set { 
                if (!string.IsNullOrEmpty(value) && value[0] == '+')
                {
                    mobilePhone = value.Substring(1);
                } else
                {
                    mobilePhone = value;
                }
            } 
        }
    }

    public class SfmcDataExtensionValues
    {
        //[JsonProperty("mobilephone")]
        //public string MobilePhone { get; set; }

        [JsonPropertyName("fromphone")]
        public string FromPhone { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("trackingid")]
        public string TrackingID { get;set; }

        [JsonPropertyName("deliverystatus")]
        public string DeliveryStatus { get; set; }

        [JsonPropertyName("twilioeventid")]
        public string EventId { get; set; }
    }
}
