using Microsoft.Extensions.Logging;
using SfmcSmsStatusReceiver.Data;
using SfmcSmsStatusReceiver.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Services
{
    public class SfmcDataLoader : IDataLoader
    {
        private readonly ILogger<FunctionLog> _log;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string SFMC_DATA_EXTKEY = EnvironmentUtils.GetEnvironmentVariable("sfmcDataExtensionExtKey", "invalid-uid");
        private static string SFMC_DATA_PKEY_NAME = EnvironmentUtils.GetEnvironmentVariable("sfmcDataExtensionPKey", "mobilephone");

        public SfmcDataLoader(ILogger<FunctionLog> logger, IHttpClientFactory httpClientFactory)
        {
            _log = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> UpsertDataAsync(IAuth authCache, SfmcDataExtension data)
        {
            var http = _httpClientFactory.CreateClient();
            var uriString = String.Concat(authCache.RestAPI, $"/hub/v1/dataevents/key:{SFMC_DATA_EXTKEY}/rows/{SFMC_DATA_PKEY_NAME}:{data.Key.MobilePhone}");

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(uriString)
            };
            
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authCache.AuthToken);
            request.Content = JsonContent.Create<SfmcDataExtension>(data);
            _log.LogInformation($"{data.Values.TrackingID} - issuing upsert request");

            var result = await http.SendAsync(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _log.LogInformation($"{data.Values.TrackingID} - upserted ({result.StatusCode})");
                return true;
            }

            _log.LogError($"{data.Values.TrackingID} - failed ({result.StatusCode} {result.ReasonPhrase})");
            return false;
        }
    }
}
