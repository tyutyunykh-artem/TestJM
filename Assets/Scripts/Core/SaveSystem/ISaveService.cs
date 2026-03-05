using Cysharp.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Core.SaveSystem
{
    /// <summary>
    /// Интерфейс для сервиса сохранений.
    /// </summary>
    public interface ISaveService
    {
        public UniTask Save(TowerState state);
        public UniTask<TowerState> Load();
        public bool HasSave();
    }
}
