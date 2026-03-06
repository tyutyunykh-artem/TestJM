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
        [Inject] private readonly IBlockAnimationService _animationService;

        public float BlockWidth => _blockPrefab.RectTransform.sizeDelta.x;
        public float BlockHeight => _blockPrefab.RectTransform.sizeDelta.y;

        public BlockView CreateScrollBlock(BlockData data, Transform parent)
        {
            BlockView block = Object.Instantiate(_blockPrefab, parent);
            ScrollBlockDragHandler handler = block.gameObject.AddComponent<ScrollBlockDragHandler>();
            _resolver.Inject(handler);
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
            TowerBlockDragHandler handler = block.gameObject.AddComponent<TowerBlockDragHandler>();
            _resolver.Inject(handler);
            block.Initialize(data);
            block.gameObject.name = $"TowerBlock_{data.ColorId}";
            return block;
        }

        public void DestroyBlock(BlockView block)
        {
            _animationService.StopAnimations(block.RectTransform);
            Object.Destroy(block.gameObject);
        }
    }
}
