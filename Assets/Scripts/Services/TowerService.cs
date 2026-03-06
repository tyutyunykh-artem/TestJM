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

        public Observable<TowerBlockEntry> OnBlockAdded => _onBlockAdded;
        public Observable<int> OnBlockRemoved => _onBlockRemoved;

        public TowerState State { get; } = new();

        public TowerBlockEntry PlaceBlock(BlockData block, float maxHorizontalOffset, float maxAbsoluteOffset)
        {
            float offset = 0f;
            if (State.Blocks.Count > 0)
            {
                float previousOffset = State.GetTop().HorizontalOffset;
                offset = previousOffset + Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
                offset = Mathf.Clamp(offset, -maxAbsoluteOffset, maxAbsoluteOffset);
            }
            TowerBlockEntry entry = new TowerBlockEntry(block, offset);
            State.AddBlock(entry);
            _onBlockAdded.OnNext(entry);
            return entry;
        }

        public void RemoveBlock(int towerIndex)
        {
            if (towerIndex < 0 || towerIndex >= State.Blocks.Count)
            {
                return;
            }

            State.RemoveAt(towerIndex);
            _onBlockRemoved.OnNext(towerIndex);
        }
    }
}
