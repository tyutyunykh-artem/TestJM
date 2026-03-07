using Cysharp.Threading.Tasks;

namespace TestGame.Core.Localization
{
    /// <summary>
    /// Интерфейс для сервиса локализации.
    /// </summary>
    public interface ILocalizationService
    {
        public UniTask<string> GetStringFromCommon(string alias);
    }
}
