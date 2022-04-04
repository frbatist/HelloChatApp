﻿using HelloChatApp.BotConsole.Abstractions;
using HelloChatApp.BotConsole.Consumers;
using HelloChatApp.BotConsole.Infra.RabbitMq;
using HelloChatApp.BotConsole.Infra.Services;
using HelloChatApp.BotConsole.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddScoped<IConsumer<StockQueryCommand>, StockQueryCommandConsumer>();
    services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
    services.AddHttpClient<IStockRepository, StockRepository>();
});

var app = builder.Build();

app.Services.Subscribe<StockQueryCommand>("stock-query-command", "stock-query-command_bot");

app.Run();