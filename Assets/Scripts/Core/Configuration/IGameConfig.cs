using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Core.Configuration
{
    /// <summary>
    /// Интерфейс для конфига списка кубов.
    /// </summary>
    public interface IGameConfig
    {
        public IReadOnlyList<BlockColorEntry> Blocks { get; }
    }

    [Serializable]
    public struct BlockColorEntry
    {
        public string Id;
        public Color Color;
    }
}
