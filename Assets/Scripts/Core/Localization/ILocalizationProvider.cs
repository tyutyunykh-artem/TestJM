namespace TestGame.Core.Localization
{
    /// <summary>
    /// Интерфейс для сервиса локализации.
    /// </summary>
    public interface ILocalizationProvider
    {
        public string GetText(string key);
    }
}
