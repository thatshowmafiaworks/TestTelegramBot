using ApiTgBot.Data;
using ApiTgBot.Models;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types;

namespace ApiTgBot.Services
{
    public class HistoryService(IDbContext context,ILogger<HistoryService> logger): IHistoryService
    {
        public async Task AddHistoryRecord(Message message)
        {
            var user = await context.GetUser(message.From.Id);
            var history = await context.GetUserHistory(user.Id);
            var userHistory = new HistoryRecord
            {
                Text = message.Text,
                HistoryId = history.Id,
                DateTime = DateTime.UtcNow
            };
            await context.AddHistoryRecord(userHistory);
        }

        //public async Task<HistoryRecord> GetHistoryRecord(Message message)
        //{
        //    var user = await context.GetUser(message.From.Id);
        //    var history = await context.GetUserHistory(user.Id);
        //}
    }
}
