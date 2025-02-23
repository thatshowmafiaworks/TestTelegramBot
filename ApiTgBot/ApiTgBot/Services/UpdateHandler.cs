using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ApiTgBot.Services
{
    public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
    {
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            logger.LogInformation($"HandleError: {exception}");

            if(exception is RequestException)
            {
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await (update switch
            {
                { Message: { } message } => OnMessage(message),
            });
        }

        private async Task OnMessage(Message message)
        {
            logger.LogInformation($"Received message:'{message.Text}' from '{message.Chat.Username}'");
            if (message.Text is not { } messageText) return;
            Message sentMessage = await (messageText.Split(' ')[0] switch
            {
                "/photo" => SendPhoto(message),
                "/getHi" => GetHi(message),
                _ => Usage(message)
            });

            logger.LogInformation($"The message was sent with text:{messageText}");
        }

        async Task<Message> Usage(Message message)
        {
            const string usage = """
                <b><u>Bot Menu</u></b>
                /getHi      - get "Hi, {username}!" text
                /photo      - get random photo
                """;

            return await bot.SendMessage(
                message.Chat, 
                usage, 
                parseMode: ParseMode.Html,
                replyMarkup: new ReplyKeyboardRemove());
        }

        async Task<Message> SendPhoto(Message message)
        {
            await bot.SendChatAction(message.Chat.Id, ChatAction.UploadPhoto);
            //await Task.Delay(1000);
            return await bot.SendPhoto(
                message.Chat.Id,
                $"https://picsum.photos/500/750?random={message.Date.Second}",
                "<a href=\"https://picsum.photos/\">picsum</a>"
                , ParseMode.Html);
        }

        async Task<Message> GetHi(Message message)
        {
            await bot.SendChatAction(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            var textToSend = $"Hi, {message.Chat.FirstName} '{message.Chat.Username}' {message.Chat.LastName} !";
            return await bot.SendMessage(
                message.Chat.Id,
                textToSend);
        }
    }
}
