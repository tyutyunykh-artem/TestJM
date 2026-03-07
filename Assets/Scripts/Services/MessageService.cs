using R3;

namespace TestGame.Services
{
    /// <summary>
    /// Сервис сообщений.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly Subject<string> _onMessage = new();

        public Observable<string> OnMessage => _onMessage;

        public void ShowMessage(string text)
        {
            _onMessage.OnNext(text);
        }
    }
}
