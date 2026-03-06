using UnityEngine;

namespace TestGame.Model
{
    /// <summary>
    /// Источник перетаскивания.
    /// </summary>
    public enum DragSource
    {
        Scroll = 0,
        Tower = 1,
    }

    /// <summary>
    /// Данные события начала перетаскивания.
    /// </summary>
    public struct DragStartedData
    {
        public BlockData BlockData;
        public Vector2 ScreenPosition;
        public DragSource Source;
        public int TowerIndex;
    }

    /// <summary>
    /// Данные события завершения перетаскивания.
    /// </summary>
    public struct DragEndedData
    {
        public BlockData BlockData;
        public Vector2 ScreenPosition;
        public DragSource Source;
        public int TowerIndex;
    }
}
