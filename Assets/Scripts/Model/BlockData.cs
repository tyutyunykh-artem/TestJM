using UnityEngine;

namespace TestGame.Model
{
    /// <summary>
    /// Модель куба.
    /// </summary>
    public class BlockData
    {
        public string ColorId { get; }
        public Color Color { get; }
        public int InstanceId { get; }

        public BlockData(string colorId, Color color, int instanceId)
        {
            ColorId = colorId;
            Color = color;
            InstanceId = instanceId;
        }
    }
}
