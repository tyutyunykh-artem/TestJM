namespace TestGame.Model
{
    /// <summary>
    /// Модель куба в башне.
    /// </summary>
    public class TowerBlockEntry
    {
        public BlockData Data { get; }
        public float HorizontalOffset { get; }

        public TowerBlockEntry(BlockData data, float horizontalOffset)
        {
            Data = data;
            HorizontalOffset = horizontalOffset;
        }
    }
}
