using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace TestGame.Core.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private const string CommonTableName = "Common";

        public async UniTask<string> GetStringFromCommon(string alias)
        {
            return await GetString(CommonTableName, alias);
        }

        private async UniTask<string> GetString(string table, string alias)
        {
            string localizedString = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, alias).Task;
            return localizedString;
        }
    }
}
