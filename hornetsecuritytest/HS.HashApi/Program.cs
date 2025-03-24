using HS.Core.Components;
using HS.Core.Interfaces;
using HS.Core.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHashGenerationService, HashGenerationService>();
builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

var app = builder.Build();

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();