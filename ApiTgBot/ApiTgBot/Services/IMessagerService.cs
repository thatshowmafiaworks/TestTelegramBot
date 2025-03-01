namespace ApiTgBot.Services
{
    public interface IMessagerService
    {
        Task<string> SendMessageToAllUsers(string textMessage);
        Task<string> SendWeatherToAllUsers();
    }
}
