using HelloChatApp.BotConsole;
using HelloChatApp.BotConsole.Abstractions;
using HelloChatApp.BotConsole.Consumers;
using HelloChatApp.BotConsole.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddScoped<IConsumer<StockQueryCommand>, StockQueryCommandConsumer>();
});

var app = builder.Build();

app.Services.Subscribe<StockQueryCommand>("stock-query-command", "stock-query-command_bot");

app.Run();