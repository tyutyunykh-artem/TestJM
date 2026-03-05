using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Core.Configuration
{
    /// <summary>
    /// Интерфейс для конфига списка кубиков.
    /// </summary>
    public interface IGameConfig
    {
        public IReadOnlyList<BlockColorEntry> Blocks { get; }
    }

    public struct BlockColorEntry
    {
        public string Id;
        public Color Color;
    }
}
