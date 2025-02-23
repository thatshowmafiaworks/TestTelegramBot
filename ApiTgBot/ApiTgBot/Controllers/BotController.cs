using ApiTgBot;
using ApiTgBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

[ApiController]
[Route("/")]
public class BotController(IOptions<BotConfiguration> Config) : ControllerBase
{
    [HttpGet]
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        var webhookurl = Config.Value.BotWebhookUrl.AbsoluteUri;
        await bot.SetWebhook(
            webhookurl,
            allowedUpdates: [],
            secretToken: Config.Value.SecretToken,
            cancellationToken: ct);
        return $"Webhook set to {webhookurl}";
    }

    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] UpdateHandler handler, CancellationToken ct)
    {
        try
        {
            await handler.HandleUpdateAsync(bot, update, ct);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Ok();
    }
}