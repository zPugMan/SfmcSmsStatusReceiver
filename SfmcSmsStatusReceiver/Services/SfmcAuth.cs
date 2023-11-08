using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SfmcSmsStatusReceiver;
using SfmcSmsStatusReceiver.Utils;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SfmcSmsStatusReceiver.Data;

namespace SfmcSmsStatusReceiver.Services
{
    public class SfmcAuth : IAuth
    {
        private static int REFRESH_OFFSET_SECS = 300;
        private static string SFMC_AUTH = EnvironmentUtils.GetEnvironmentVariable("sfmcAuth", "http://test.com");

        private readonly ILogger<FunctionLog> _log;
        private readonly IHttpClientFactory _httpClientFactory;
        public Task Initialize { get; set; }

        public string? AuthToken { get; private set; }
        public DateTime? RefreshExpiry { get; private set; }
        public string? RestAPI { get; private set; }

        public SfmcAuth(ILogger<FunctionLog> logger, IHttpClientFactory httpFactory)
        {
            _log = logger;
            _httpClientFactory = httpFactory;
            Initialize = AuthorizeAsync();
            _log.LogInformation($"{nameof(SfmcAuth)} service initialized");
        }

        private void SetAuthorization(string accessToken, int refreshExpirySeconds)
        {
            AuthToken = accessToken;
            RefreshExpiry = DateTime.UtcNow + TimeSpan.FromSeconds(refreshExpirySeconds - REFRESH_OFFSET_SECS);
            _log.LogInformation($"Setting authorization values with refresh expiry: {RefreshExpiry:O}");
        }

        public bool RequireAuth()
        {
            if (string.IsNullOrEmpty(AuthToken))
            {
                _log.LogWarning("AuthToken is null");
                return true;
            }

            if (!RefreshExpiry.HasValue)
            {
                _log.LogWarning("RefreshExpiry is null");
                return true;
            }

            if (RefreshExpiry.Value <= DateTime.UtcNow)
            {
                _log.LogWarning($"RefreshExpiry is past due ({RefreshExpiry.Value:O} <= {DateTime.UtcNow:O})");
                return true;
            }

            _log.LogDebug("No Re-Auth required");
            return false;
        }

        public async Task<bool> AuthorizeAsync()
        {
            var http = _httpClientFactory.CreateClient();
            
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(SFMC_AUTH),
            };

            request.Content = JsonContent.Create(new
            {
                grant_type = "client_credentials",
                client_id = Environment.GetEnvironmentVariable("sfmcAuthClientId", EnvironmentVariableTarget.Process),
                client_secret = Environment.GetEnvironmentVariable("sfmcAuthClientSecret", EnvironmentVariableTarget.Process),
            });

            _log.LogInformation($"Requesting Auth: {SFMC_AUTH}");
            var result = await http.SendAsync(request);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _log.LogInformation($"Authorization received ({result.StatusCode})");
                var body = result.Content.ReadAsStringAsync().Result;
                var authResponse = JsonConvert.DeserializeObject<SfmcAuthResponse>(body);

                if (authResponse == null)
                    return false;
                else
                {
                    SetAuthorization(authResponse.AccessToken, authResponse.ExpirySeconds);
                    RestAPI = authResponse.RestApi;
                }

                return true;
            }
            else
            {
                _log.LogError($"Authorization failed ({result.StatusCode}). {result.ReasonPhrase}");
                return false;
            }

        }
    }
}
