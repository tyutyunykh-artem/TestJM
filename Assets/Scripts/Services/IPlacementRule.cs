using TestGame.Model;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс правил размещения кубов в башне.
    /// </summary>
    public interface IPlacementRule
    {
        public bool CanPlace(BlockData newBlock, TowerState tower);
    }
}
