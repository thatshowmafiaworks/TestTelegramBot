using ApiTgBot.Data;
using ApiTgBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ApiTgBot.Services
{
    public class UpdateHandler(ITelegramBotClient bot, 
            ILogger<UpdateHandler> logger, 
            IDbContext context,
            IWeatherService weather) : IUpdateHandler
    {
        private readonly Dictionary<long, bool> awaitingCityText = new Dictionary<long, bool>();
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            logger.LogInformation($"HandleError: {exception}");

            if (exception is RequestException)
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
            if(message.Type == MessageType.Location)
            {
                await UpdateLocation(message);
                return;
            }
            if (message.Text is not { } messageText) return;
            var user = await context.GetUser(message.From.Id);
            if (user is null) await context.AddUser(
                new Models.User
                {
                    Id = message.From.Id,
                    Username = message.From.Username,
                    FirstName = message.From.FirstName,
                    LastName = message.From.LastName
                });

            Message sentMessage = await (messageText.Split(' ')[0] switch
            {
                "/photo" => SendPhoto(message),
                "/getHi" => GetHi(message),
                "/weatherByCity" => GetWeatherByCity(message),
                "/city" => SetCity(message),
                "/weatherByCoordinates" => GetWeatherByLocation(message),
                _ => Usage(message)
            });
            
            logger.LogInformation($"The message was sent with text:{messageText}");
        }

        async Task<Message> SetCity(Message message)
        {
            await bot.SendChatAction(message.Chat.Id, ChatAction.Typing);
            awaitingCityText[message.From.Id] = true;
            return await bot.SendMessage(
                message.Chat.Id,
                "Send your city name pls");
        }

        async Task<Message> Usage(Message message)
        {
            if (awaitingCityText.ContainsKey(message.From.Id) && awaitingCityText[message.From.Id])
            {
                //if awaiting for city creating new city
                var user = await context.GetUser(message.From.Id);
                var city = await context.GetCity(message.Text);
                if (city == null)
                {
                    city = new Models.City { Name = message.Text };
                    await context.AddCity(message.Text);
                    city = await context.GetCity(message.Text);
                }
                user.CityId = city.Id;
                await context.UpdateUser(user);
                awaitingCityText[message.From.Id] = false;
                return await bot.SendMessage(message.Chat, "City was added");
            }

            const string usage = """
            <b><u>Bot Menu</u></b>
            /getHi              - get "Hi, {username}!" text
            /photo              - get random photo
            /weatherByCity      - get weather for your city
            /weatherByCoordinates  - get weather for your location
            /city               - set your city
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

        async Task<Message> UpdateLocation(Message message)
        {
            throw new NotImplementedException();
        }

        async Task<Message> GetWeatherByCity(Message message)
        {
            throw new NotImplementedException("GetWeatherByCity");
        }
        async Task<Message> GetWeatherByLocation(Message message)
        {
            if (!await IsUserHaveLocation(message))
            {
                return await bot.SendMessage(message.Chat.Id,"Please send your location to the chat");
            }
            var coordinates = await context.GetCoordinates(message.From.Id);
            var forecast = await weather.GetForecast(coordinates);
            return await bot.SendMessage(message.Chat.Id, forecast);
        }

        private async Task<bool> IsUserHaveLocation(Message message)
        {
            Models.User user = await context.GetUser(message.From.Id);
            return (user.Lat == null && user.Lng == null && user.Lat == 0 && user.Lng == 0) ? false : true;
        }
    }
}
