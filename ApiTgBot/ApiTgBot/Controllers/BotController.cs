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
public class BotController(IOptions<BotConfiguration> Config, IConfiguration config, IDbContext context,IMessagerService messager) : ControllerBase
{

    /// <summary>
    /// Setting WebHook so Telegram would know where to send updates
    /// </summary>
    /// <returns>String with success message</returns>
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

    /// <summary>
    /// Route to receive Updates from Telegram 
    /// </summary>
    /// <param name="update">Update with message or action from user</param>
    /// <param name="bot"></param>
    /// <param name="handler"></param>
    /// <param name="ct"></param>
    /// <returns>200 status</returns>
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
    /// <summary>
    /// Return user info from DB
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>ApiTgBot.Models.User from DB</returns>
    [HttpGet]
    [Route("user/{id}")]
    public async Task<ApiTgBot.Models.User> GetUser(long id)
    {
        var user = await context.GetUser(id);
        var histories = await context.GetHistoryRecordsForUser(id);
        user.Histories = histories;
        return user;
    }

    /// <summary>
    /// Triggers messager that send weather message to all users
    /// </summary>
    /// <returns>Success string</returns>
    [HttpPost]
    [Route("sendweather/")]
    public async Task<string> SendWeatherToAll()
    {
        var result = await messager.SendWeatherToAllUsers();
        return result;
    }

    /// <summary>
    /// Triggers messager that send text message to all users
    /// </summary>
    /// <param name="textMessage">message to send, support html markup</param>
    /// <returns></returns>
    [HttpPost]
    [Route("sendmessage/")]
    public async Task<string> SendMessageToAll([FromBody]string textMessage)
    {
        var result = await messager.SendMessageToAllUsers(textMessage);
        return result;
    }
}