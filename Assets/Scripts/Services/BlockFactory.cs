using TestGame.Model;
using TestGame.Views;
using UnityEngine;
using VContainer;

namespace TestGame.Services
{
    /// <summary>
    /// Фабрика для создания View-кубов из префаба.
    /// </summary>
    public class BlockFactory : IBlockFactory
    {
        [Inject] private readonly BlockView _blockPrefab;

        public BlockView CreateScrollBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            block.Initialize(data);
            block.gameObject.name = $"ScrollBlock_{data.ColorId}";
            return block;
        }

        public BlockView CreateDraggableBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            block.Initialize(data);
            block.gameObject.name = $"DragBlock_{data.ColorId}";
            return block;
        }

        public BlockView CreateTowerBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            block.Initialize(data);
            block.gameObject.name = $"TowerBlock_{data.ColorId}";
            return block;
        }

        public void ReturnToPool(BlockView block)
        {
            // TODO

            Object.Destroy(block.gameObject);
        }
    }
}
