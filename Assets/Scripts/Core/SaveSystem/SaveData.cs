using System;
using System.Collections.Generic;

namespace TestGame.Core.SaveSystem
{
    /// <summary>
    /// Модель для сохранения состояния башни.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public List<SavedBlockEntry> TowerBlocks = new();
    }

    /// <summary>
    /// Модель куба в башне для сохранения.
    /// </summary>
    [Serializable]
    public class SavedBlockEntry
    {
        public string ColorId;
        public float HorizontalOffset;
    }
}
