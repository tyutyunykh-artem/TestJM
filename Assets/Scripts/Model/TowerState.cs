using System.Collections.Generic;
using System.Linq;
using R3;

namespace TestGame.Model
{
    /// <summary>
    /// Модель состояния башни.
    /// </summary>
    public class TowerState
    {
        private readonly List<TowerBlockEntry> _blocks = new();

        public IReadOnlyList<TowerBlockEntry> Blocks => _blocks;
        public ReactiveProperty<int> BlockCount { get; } = new(0);

        public void AddBlock(TowerBlockEntry entry)
        {
            _blocks.Add(entry);
            BlockCount.Value = _blocks.Count;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _blocks.Count)
            {
                return;
            }

            _blocks.RemoveAt(index);
            BlockCount.Value = _blocks.Count;
        }

        public void Clear()
        {
            _blocks.Clear();
            BlockCount.Value = 0;
        }

        public TowerBlockEntry GetTop()
        {
            return _blocks.Count > 0 ? _blocks.Last() : null;
        }
    }
}
