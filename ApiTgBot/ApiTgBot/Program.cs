using ApiTgBot;
using ApiTgBot.Data;
using ApiTgBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
builder.Services.AddSwaggerGen(opts =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
    opts.EnableAnnotations();
    opts.CustomSchemaIds(type => type.FullName);
    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Telegram Weather Bot",
        Description = "Telegram Bot that give you weather in your city or your location using OpenWeather API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "GitHub",
            Url = new Uri("https://github.com/thatshowmafiaworks/TestTelegramBot")
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.MapControllers();

app.Run();


