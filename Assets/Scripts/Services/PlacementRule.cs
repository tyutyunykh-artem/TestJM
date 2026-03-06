using TestGame.Model;

namespace TestGame.Services
{
    /// <summary>
    /// Правила размещения кубов в башне.
    /// </summary>
    public class PlacementRule : IPlacementRule
    {
        public bool CanPlace(BlockData newBlock, TowerState tower)
        {
            return true; // Разрешает любое размещение кубов
        }
    }
}
