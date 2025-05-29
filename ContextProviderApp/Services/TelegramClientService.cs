using ContextProviderApp.Interfaces;

namespace ContextProviderApp.Services
{
    public class TelegramClientService
    {
        private readonly ITelegramClient _telegramClient;
        public TelegramClientService(ITelegramClient telegramClient)
        {
            _telegramClient = telegramClient;
        }
        public void SendMessageToChannel()
        {

        }
    }
}