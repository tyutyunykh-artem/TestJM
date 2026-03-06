using R3;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс сервиса сообщений.
    /// </summary>
    public interface IMessageService
    {
        public Observable<string> OnMessage { get; }
        public void ShowMessage(string text);
    }
}
