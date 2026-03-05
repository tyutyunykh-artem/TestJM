using UnityEngine;
using TestGame.Model;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс для фабрики создания View-кубов.
    /// </summary>
    public interface IBlockFactory
    {
        public GameObject CreateDraggableBlock(BlockData data, Transform parent);
        public GameObject CreateTowerBlock(BlockData data, Transform parent);
        public void ReturnToPool(GameObject blockObject);
    }
}
