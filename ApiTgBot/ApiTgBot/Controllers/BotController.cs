using ApiTgBot;
using ApiTgBot.Data;
using ApiTgBot.Models;
using ApiTgBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

[ApiController]
[Route("/")]
public class BotController(IOptions<BotConfiguration> Config, IConfiguration config, IDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        var webhookurl = config["BotWebhookUrl"];
        await bot.SetWebhook(
            webhookurl,
            allowedUpdates: [],
            secretToken: Config.Value.SecretToken,
            cancellationToken: ct);
        return $"Webhook set to {webhookurl}";
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] UpdateHandler handler, CancellationToken ct)
    {
        try
        {
            await handler.HandleUpdateAsync(bot, update, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Ok();
    }

    [HttpGet]
    [Route("/user/")]
    public async Task<ApiTgBot.Models.User> GetUser(long id)
    {
        var user = await context.GetUser(id);
        return user;
    }

    [HttpPost]
    [Route("/sendweather/")]
    public async Task<IActionResult> SendWeatherToAll()
    {
        throw new NotImplementedException();
    }
}