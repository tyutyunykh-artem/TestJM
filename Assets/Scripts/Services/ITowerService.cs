using R3;
using UnityEngine;
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
        public Observable<string> OnMessage { get; }

        public TowerState State { get; }

        public bool CanPlaceBlock(Vector2 screenPosition);
        public void PlaceBlock(BlockData block, Vector2 dropScreenPosition);
        public void RemoveBlock(int towerIndex);
        public bool IsTowerAtMaxHeight();
    }
}
