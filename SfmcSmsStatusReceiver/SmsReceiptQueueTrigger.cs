using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SfmcSmsStatusReceiver;
using SfmcSmsStatusReceiver.Data;
using SfmcSmsStatusReceiver.Services;

namespace Solvenna.Function
{
    public class SmsReceiptQueueTrigger
    {
        private readonly ILogger<FunctionLog> _log;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IAuth _sfmcAuth;

        public SmsReceiptQueueTrigger(ILogger<FunctionLog> logger, IHttpClientFactory httpClientFactory, IAuth sfmcAuth)
        {
            _log = logger;
            _sfmcAuth = sfmcAuth;
            _httpFactory = httpClientFactory;
        }

        [Function(nameof(SmsReceiptQueueTrigger))]
        public async Task<bool> Run(
            [ServiceBusTrigger("%receipt_QUEUE%", Connection = "receipt_SERVICEBUS")] 
            ServiceBusReceivedMessage msg,
            ServiceBusMessageActions messageActions
            )
        {
            try
            {
                await _sfmcAuth.Initialize;

                if (_sfmcAuth.RequireAuth())
                    await _sfmcAuth.AuthorizeAsync();

                _log.LogInformation("Message ID: {id}", msg.MessageId);
                _log.LogInformation("Message Body: {body}", msg.Body);  //TODO remove
                _log.LogInformation("Message Content-Type: {contentType}", msg.ContentType);

                if (msg.ContentType != "application/json")
                {
                    _log.LogError($"Expecting JSON content. Received {msg.ContentType}");
                    await messageActions.DeadLetterMessageAsync(msg, deadLetterReason: "ContentType not 'application/json'");
                    return false;
                }

                var received = JsonConvert.DeserializeObject<QueueMessage>(Encoding.UTF8.GetString(msg.Body));
                var sendReq = new SfmcDataExtension(received);

                if(!sendReq.IsValidForUpload())
                {
                    _log.LogWarning("Received message is missing minimum required data attributes");
                    await messageActions.DeadLetterMessageAsync(msg, deadLetterReason: "Missing required attributes");
                    return false;
                }

                var loader = new SfmcDataLoader(_log, _httpFactory);
                var result = await loader.UpsertDataAsync(_sfmcAuth, sendReq);

                await messageActions.CompleteMessageAsync(msg);
                return result;
            } catch(Exception ex )
            {
                _log.LogError($"Failed to process message: {msg.MessageId}", ex);
                await messageActions.DeadLetterMessageAsync(msg, deadLetterReason: ex.Message);
                return false;
            }
        }
    }
}
