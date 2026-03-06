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
        private readonly Subject<TowerState> _onStateRestored = new();

        private float _maxHorizontalOffset;

        public Observable<TowerBlockEntry> OnBlockAdded => _onBlockAdded;
        public Observable<int> OnBlockRemoved => _onBlockRemoved;
        public Observable<TowerState> OnStateRestored => _onStateRestored;

        public TowerState State { get; } = new();

        public TowerBlockEntry PlaceBlock(BlockData block, float maxHorizontalOffset, float maxAbsoluteOffset)
        {
            _maxHorizontalOffset = maxHorizontalOffset;

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

        public void RemoveBlock(int towerIndex, float maxHorizontalOffset)
        {
            if (towerIndex < 0 || towerIndex >= State.Blocks.Count)
            {
                return;
            }

            _maxHorizontalOffset = maxHorizontalOffset;
            State.RemoveAt(towerIndex);
            RecalculateOffsets();
            _onBlockRemoved.OnNext(towerIndex);
        }

        public void RestoreState(TowerState loadedState)
        {
            State.Clear();
            foreach (TowerBlockEntry entry in loadedState.Blocks)
            {
                State.AddBlock(entry);
            }

            _onStateRestored.OnNext(State);
        }

        private void RecalculateOffsets()
        {
            for (int i = 1; i < State.Blocks.Count; i++)
            {
                TowerBlockEntry current = State.Blocks[i];
                TowerBlockEntry below = State.Blocks[i - 1];
                float diff = current.HorizontalOffset - below.HorizontalOffset;

                if (Mathf.Abs(diff) > _maxHorizontalOffset)
                {
                    float clampedDiff = Mathf.Clamp(diff, -_maxHorizontalOffset, _maxHorizontalOffset);
                    current.SetHorizontalOffset(below.HorizontalOffset + clampedDiff);
                }
            }
        }
    }
}
