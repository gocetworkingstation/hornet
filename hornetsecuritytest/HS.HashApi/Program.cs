using HS.Core.Components;
using HS.Core.Data;
using HS.Core.Interfaces;
using HS.Core.Repositories;
using HS.Core.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new ConnectionFactory { HostName = "localhost" });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHashGenerationService, HashGenerationService>();
builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IHashRepository, HashRepository>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

var app = builder.Build();

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();