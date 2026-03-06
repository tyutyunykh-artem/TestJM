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
        [Inject] private readonly IObjectResolver _resolver;

        public BlockView CreateScrollBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            InjectComponents(block.gameObject);
            block.Initialize(data);
            block.gameObject.name = $"ScrollBlock_{data.ColorId}";
            return block;
        }

        public BlockView CreateDraggableBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            RemoveScrollDragHandler(block.gameObject);
            block.Initialize(data);
            block.gameObject.name = $"DragBlock_{data.ColorId}";
            return block;
        }

        public BlockView CreateTowerBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            RemoveScrollDragHandler(block.gameObject);
            block.Initialize(data);
            block.gameObject.name = $"TowerBlock_{data.ColorId}";
            return block;
        }

        public void ReturnToPool(BlockView block)
        {
            // TODO

            Object.Destroy(block.gameObject);
        }

        private void InjectComponents(GameObject gameObject)
        {
            MonoBehaviour[] components = gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                _resolver.Inject(component);
            }
        }

        private void RemoveScrollDragHandler(GameObject gameObject)
        {
            ScrollBlockDragHandler handler = gameObject.GetComponent<ScrollBlockDragHandler>();
            if (handler != null)
            {
                Object.Destroy(handler);
            }
        }
    }
}
