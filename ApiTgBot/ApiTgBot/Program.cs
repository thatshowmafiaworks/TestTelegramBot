using ApiTgBot;
using ApiTgBot.Data;
using ApiTgBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

var botConfigSection = builder.Configuration.GetSection("BotConfiguration");
builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<BotConfiguration>(botConfigSection);

builder.Services.AddHttpClient("tgwebhook").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>(
    httpClient => new TelegramBotClient(builder.Configuration["BotToken"], httpClient));
builder.Services.AddSingleton<UpdateHandler>();
builder.Services.AddSingleton<IWeatherService,WeatherService>();
builder.Services.AddSingleton<IHistoryService, HistoryService>();
builder.Services.AddSingleton<IMessagerService, MessagerService>();
builder.Services.AddSingleton<IDbContext, DbContext>();

builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


