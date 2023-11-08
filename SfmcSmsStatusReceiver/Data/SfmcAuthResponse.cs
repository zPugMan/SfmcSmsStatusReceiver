using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SfmcSmsStatusReceiver.Data
{
    public class SfmcAuthResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpirySeconds { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("rest_instance_url")]
        public string RestApi { get; set; }
    }
}
