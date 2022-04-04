using HelloChatApp.BotConsole.Consumers;
using HelloChatApp.BotConsole.Domain.Abstractions;
using HelloChatApp.BotConsole.Infra.Services;
using HelloChatApp.BotConsole.Services;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Infra.RabbitMq;
using HelloChatApp.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddScoped<IConsumer<StockQueryCommand>, StockQueryCommandConsumer>();
    services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
    services.AddHttpClient<IStockRepository, StockRepository>();
    services.AddScoped<IStockService, StockService>();
});

var app = builder.Build();

app.Services.Subscribe<StockQueryCommand>("stock-query-command", "stock-query-command_bot");

app.Run();