using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Tests.Services
{
    public class MockHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(MockSend(request, cancellationToken));
        }
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return MockSend(request, cancellationToken);
        }

        public virtual HttpResponseMessage MockSend(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
