namespace TestGame.Model
{
    /// <summary>
    /// Модель куба в башне.
    /// </summary>
    public class TowerBlockEntry
    {
        public BlockData Data { get; }
        public float HorizontalOffset { get; private set; }

        public TowerBlockEntry(BlockData data, float horizontalOffset)
        {
            Data = data;
            HorizontalOffset = horizontalOffset;
        }

        public void SetHorizontalOffset(float offset)
        {
            HorizontalOffset = offset;
        }
    }
}
