using HS.Core.Components;
using HS.Core.Data;
using HS.Core.Interfaces;
using HS.Core.Repositories;
using HS.HashProcessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(new ConnectionFactory { HostName = "localhost" });
        services.AddScoped<IMessageConsumer, RabbitMqConsumer>();
        services.AddScoped<IHashRepository, HashRepository>();
        services.AddHostedService<HashProcessorWorker>();
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
    })
    .Build()
    .Run();