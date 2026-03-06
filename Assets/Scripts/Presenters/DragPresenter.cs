using R3;
﻿using Cysharp.Threading.Tasks;
using System;
using TestGame.Model;
using TestGame.Services;
using TestGame.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestGame.Presenters
{
    /// <summary>
    /// Презентер перетаскивания кубов.
    /// </summary>
    public class DragPresenter : IStartable, IDisposable
    {
        [Inject] private readonly IDragMediator _dragMediator;
        [Inject] private readonly IBlockFactory _blockFactory;
        [Inject] private readonly ITowerService _towerService;
        [Inject] private readonly IBlockAnimationService _animationService;
        [Inject] private readonly ScrollAreaView _scrollAreaView;
        [Inject] private readonly TowerAreaView _towerAreaView;
        [Inject] private readonly HoleView _holeView;

        private readonly CompositeDisposable _disposables = new();

        private Canvas _canvas;
        private RectTransform _canvasRect;
        private Camera _canvasCamera;
        private BlockView _currentClone;

        public void Start()
        {
            _canvas = _scrollAreaView.GetComponentInParent<Canvas>().rootCanvas;
            _canvasRect = _canvas.GetComponent<RectTransform>();
            _canvasCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

            _dragMediator.OnDragStarted.Subscribe(OnDragStarted).AddTo(_disposables);
            _dragMediator.OnDragUpdated.Subscribe(OnDragUpdated).AddTo(_disposables);
            _dragMediator.OnDragEnded.Subscribe(OnDragEnded).AddTo(_disposables);
        }

        private void OnDragStarted(DragStartedData data)
        {
            _currentClone = _blockFactory.CreateDraggableBlock(data.BlockData, _canvasRect);
            MoveCloneToScreenPosition(data.ScreenPosition);
        }

        private void OnDragUpdated(Vector2 screenPosition)
        {
            if (_currentClone == null)
            {
                return;
            }

            MoveCloneToScreenPosition(screenPosition);
        }

        private void OnDragEnded(DragEndedData data)
        {
            if (_currentClone == null)
            {
                return;
            }

            BlockView clone = _currentClone;
            _currentClone = null;

            HandleDropAsync(data, clone).Forget();
        }

        private async UniTaskVoid HandleDropAsync(DragEndedData data, BlockView clone)
        {
            if (data.Source == DragSource.Scroll)
            {
                await HandleScrollBlockDrop(data, clone);
            }
            else if (data.Source == DragSource.Tower)
            {
                await HandleTowerBlockDrop(data, clone);
            }
        }

        private async UniTask HandleScrollBlockDrop(DragEndedData data, BlockView clone)
        {
            bool isInTowerZone = RectTransformUtility.RectangleContainsScreenPoint(_towerAreaView.TowerZoneRect, data.ScreenPosition, _canvasCamera);

            if (isInTowerZone && !IsTowerAtMaxHeight())
            {
                _blockFactory.ReturnToPool(clone);
                float maxOffset = _blockFactory.BlockWidth * 0.5f;
                float halfZoneWidth = _towerAreaView.TowerZoneRect.rect.width * 0.5f - _blockFactory.BlockWidth * 0.5f;
                _towerService.PlaceBlock(data.BlockData, maxOffset, halfZoneWidth);
            }
            else
            {
                await _animationService.PlayDisappear(clone.RectTransform);
                _blockFactory.ReturnToPool(clone);
            }
        }

        private async UniTask HandleTowerBlockDrop(DragEndedData data, BlockView clone)
        {
            if (IsInsideHoleEllipse(data.ScreenPosition))
            {
                _blockFactory.ReturnToPool(clone);
                _towerService.RemoveBlock(data.TowerIndex);
            }
            else
            {
                await _animationService.PlayDisappear(clone.RectTransform);
                _blockFactory.ReturnToPool(clone);
            }
        }

        private bool IsTowerAtMaxHeight()
        {
            float blockHeight = _blockFactory.BlockHeight;
            float currentHeight = _towerService.State.Blocks.Count * blockHeight;
            float nextHeight = currentHeight + blockHeight;
            float availableHeight = _towerAreaView.TowerZoneRect.rect.height;
            return nextHeight > availableHeight;
        }

        private bool IsInsideHoleEllipse(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_holeView.HoleImageRect, screenPosition, _canvasCamera, out Vector2 localPoint);

            Vector2 size = _holeView.HoleImageRect.rect.size;
            float rx = size.x * 0.5f;
            float ry = size.y * 0.5f;

            if (rx <= 0f || ry <= 0f)
            {
                return false;
            }

            return (localPoint.x * localPoint.x) / (rx * rx) + (localPoint.y * localPoint.y) / (ry * ry) <= 1f;
        }

        private void MoveCloneToScreenPosition(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPosition, _canvasCamera, out Vector2 localPoint);

            _currentClone.RectTransform.localPosition = localPoint;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
