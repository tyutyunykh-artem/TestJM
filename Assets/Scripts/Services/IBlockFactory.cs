using TestGame.Model;
using TestGame.Views;
using UnityEngine;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс для фабрики создания View-кубов.
    /// </summary>
    public interface IBlockFactory
    {
        public BlockView CreateScrollBlock(BlockData data, Transform parent);
        public BlockView CreateDraggableBlock(BlockData data, Transform parent);
        public BlockView CreateTowerBlock(BlockData data, Transform parent);
        public void ReturnToPool(BlockView block);
    }
}
