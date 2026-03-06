using System.Collections.Generic;
using System.Linq;

namespace TestGame.Model
{
    /// <summary>
    /// Модель состояния башни.
    /// </summary>
    public class TowerState
    {
        private readonly List<TowerBlockEntry> _blocks = new();

        public IReadOnlyList<TowerBlockEntry> Blocks => _blocks;

        public void AddBlock(TowerBlockEntry entry)
        {
            _blocks.Add(entry);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _blocks.Count)
            {
                return;
            }

            _blocks.RemoveAt(index);
        }

        public void Clear()
        {
            _blocks.Clear();
        }

        public TowerBlockEntry GetTop()
        {
            return _blocks.Count > 0 ? _blocks.Last() : null;
        }
    }
}
