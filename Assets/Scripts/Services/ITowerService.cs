using R3;
using TestGame.Model;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс для сервиса управления башней.
    /// </summary>
    public interface ITowerService
    {
        public Observable<TowerBlockEntry> OnBlockAdded { get; }
        public Observable<int> OnBlockRemoved { get; }

        public TowerState State { get; }

        public TowerBlockEntry PlaceBlock(BlockData block, float maxHorizontalOffset, float maxAbsoluteOffset);
        public void RemoveBlock(int towerIndex);
    }
}
