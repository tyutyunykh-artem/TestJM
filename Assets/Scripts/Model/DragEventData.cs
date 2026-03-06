using UnityEngine;

namespace TestGame.Model
{
    /// <summary>
    /// Данные события начала перетаскивания.
    /// </summary>
    public struct DragStartedData
    {
        public BlockData BlockData;
        public Vector2 ScreenPosition;
    }

    /// <summary>
    /// Данные события завершения перетаскивания.
    /// </summary>
    public struct DragEndedData
    {
        public BlockData BlockData;
        public Vector2 ScreenPosition;
    }
}
