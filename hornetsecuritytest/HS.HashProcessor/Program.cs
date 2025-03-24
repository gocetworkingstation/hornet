using HS.Core.Components;
using HS.Core.Interfaces;
using HS.Core.Repositories;
using HS.HashProcessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(new ConnectionFactory { HostName = "localhost" });
        services.AddScoped<IMessageConsumer, RabbitMqConsumer>();
        services.AddScoped<IHashRepository, HashRepository>();
        services.AddHostedService<HashProcessorWorker>();
    })
    .Build()
    .Run();