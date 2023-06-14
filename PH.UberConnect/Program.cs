using PH.UberConnect;
using PH.UberConnect.Api;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Extensions;
using PH.UberConnect.Settings;

SerilogExtension.AddSerilog();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddWindowsService(options =>
        {
            options.ServiceName = "PH Uber Direct";
        });

        IConfiguration configuration = hostContext.Configuration;
        ApiSettings apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
        Apis api = new(apiSettings.Url, apiSettings.AuthUrl, apiSettings.CustomerId);
        Services servicesCore = new(configuration.GetConnectionString("PHCRSRVMCARIIS0"));
        services.AddSingleton(servicesCore);
        services.AddSingleton(apiSettings);
        services.AddSingleton(api);
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();
