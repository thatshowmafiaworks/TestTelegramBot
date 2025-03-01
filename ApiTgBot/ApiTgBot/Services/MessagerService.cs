using ApiTgBot.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ApiTgBot.Services
{
    public class MessagerService(IDbContext context, ITelegramBotClient bot,UpdateHandler handler) : IMessagerService
    {
        public async Task<string> SendWeatherToAllUsers()
        {
            var users = await context.GetAllUsers();
            foreach(var user in users)
            {
                if ((user.Lat == 0 || user.Lng == 0) && user.CityId == 0) continue;
                var message = new Message()
                {
                    Chat = new Chat { Id = user.ChatId },
                    From = new User { Id = user.Id }
                };
                if (user.Lat != 0 && user.Lng != 0)
                    await handler.GetWeatherByLocation(message);
                if (user.CityId != 0) await handler.GetWeatherByCity(message);
            }

            return "Weather was sent to all users";
        }
        public async Task<string> SendMessageToAllUsers(string textMessage)
        {
            var users = await context.GetAllUsers();
            foreach (var user in users)
            {
                var message = new Message()
                {
                    Chat = new Chat { Id = user.ChatId },
                    From = new User { Id = user.Id }
                };
                await bot.SendMessage(user.ChatId, textMessage,parseMode:ParseMode.Html);
            }
            return $"Message '{textMessage}' was sent to all users";
        }
    }
}
