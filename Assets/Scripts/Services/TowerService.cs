using R3;
using UnityEngine;
using TestGame.Model;

namespace TestGame.Services
{
    /// <summary>
    /// Сервис управления башней.
    /// </summary>
    public class TowerService : ITowerService
    {
        private readonly Subject<TowerBlockEntry> _onBlockAdded = new();
        private readonly Subject<int> _onBlockRemoved = new();
        private readonly Subject<string> _onMessage = new();

        public Observable<TowerBlockEntry> OnBlockAdded => _onBlockAdded;
        public Observable<int> OnBlockRemoved => _onBlockRemoved;
        public Observable<string> OnMessage => _onMessage;

        public TowerState State { get; } = new();

        public bool CanPlaceBlock(Vector2 screenPosition)
        {
            // TODO

            return false;
        }

        public void PlaceBlock(BlockData block, Vector2 dropScreenPosition)
        {
            // TODO
        }

        public void RemoveBlock(int towerIndex)
        {
            // TODO
        }

        public bool IsTowerAtMaxHeight()
        {
            // TODO

            return false;
        }
    }
}
