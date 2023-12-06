using Microsoft.Extensions.Logging;
using SfmcSmsStatusReceiver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net.Http.Json;

namespace SfmcSmsStatusReceiver.Tests.Services
{
    [TestClass]
    public class SfmcAuthTests
    {
        private ILogger<FunctionLog> _logger;
        private SfmcAuth _sfmcAuth;
        private readonly Mock<HttpMessageHandler> _handler;
        private IHttpClientFactory _httpClientFactory;

        public SfmcAuthTests()
        {
            _handler = new Mock<HttpMessageHandler>();
        }

        [TestInitialize]
        public void Setup()
        {
            using ILoggerFactory logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = logFactory.CreateLogger<FunctionLog>();

            
            _httpClientFactory = _handler.CreateClientFactory();
            _sfmcAuth = new SfmcAuth(_logger, _httpClientFactory);
        }

        [TestMethod]
        public async Task AuthorizeAsync_OKTest()
        {
            //arrange
            var authResponse = """
            {
                "access_token": "access_token123",
                "token_type": "Bearer",
                "expires_in": 1000,
                "rest_instance_url": "https://my-rest-api.com/services"
            }
            """;

            var authResponseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            authResponseMsg.Content = new StringContent(authResponse,Encoding.UTF8, "application/json");

           _handler.SetupAnyRequest().ReturnsAsync(authResponseMsg);

            //act
            var result = await _sfmcAuth.AuthorizeAsync();

            //assert
            Assert.IsTrue(result, "AuthorizeAsync failed");
            Assert.IsNotNull(_sfmcAuth.AuthToken);
        }
    }
}
