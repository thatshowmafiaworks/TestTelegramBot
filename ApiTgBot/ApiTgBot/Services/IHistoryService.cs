using Telegram.Bot.Types;

namespace ApiTgBot.Services
{
    public interface IHistoryService
    {
        Task AddHistoryRecord(Message message);
    }
}
