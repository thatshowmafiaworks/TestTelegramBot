namespace ApiTgBot
{
    public class BotConfiguration
    {
        public string BotToken { get; set; } = default;
        public Uri BotWebhookUrl { get; set; } = default;
        public string SecretToken { get; set; } = default;
    }
}
