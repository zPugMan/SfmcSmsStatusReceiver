using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SfmcSmsStatusReceiver.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddSingleton<IAuth, SfmcAuth>();
    })
    .Build();

host.Run();
