using R3;
using Cysharp.Threading.Tasks;
using System;
using TestGame.Core.Localization;
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
        [Inject] private readonly IMessageService _messageService;
        [Inject] private readonly IPlacementRule _placementRule;
        [Inject] private readonly ILocalizationService _localizationService;

        private readonly CompositeDisposable _disposables = new();

        private Canvas _canvas;
        private RectTransform _canvasRect;
        private Camera _canvasCamera;
        private BlockView _currentClone;
        private bool _isProcessingDrop;

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
            if (_isProcessingDrop)
            {
                return;
            }

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
            _isProcessingDrop = true;

            if (data.Source == DragSource.Scroll)
            {
                await HandleScrollBlockDrop(data, clone);
            }
            else if (data.Source == DragSource.Tower)
            {
                await HandleTowerBlockDrop(data, clone);
            }

            _isProcessingDrop = false;
        }

        private async UniTask HandleScrollBlockDrop(DragEndedData data, BlockView clone)
        {
            bool isInTowerZone = RectTransformUtility.RectangleContainsScreenPoint(_towerAreaView.TowerZoneRect, data.ScreenPosition, _canvasCamera);

            if (isInTowerZone && !IsTowerAtMaxHeight() && _placementRule.CanPlace(data.BlockData, _towerService.State))
            {
                _blockFactory.ReturnToPool(clone);
                float maxOffset = _blockFactory.BlockWidth * 0.5f;
                float halfZoneWidth = _towerAreaView.TowerZoneRect.rect.width * 0.5f - _blockFactory.BlockWidth * 0.5f;
                _towerService.PlaceBlock(data.BlockData, maxOffset, halfZoneWidth);
                ShowLocalizedMessage(LocKeys.BlockPlaced).Forget();
            }
            else if (isInTowerZone && !_placementRule.CanPlace(data.BlockData, _towerService.State))
            {
                ShowLocalizedMessage(LocKeys.ChooseAnotherBlock).Forget();
                await _animationService.PlayDisappear(clone.RectTransform);
                _blockFactory.ReturnToPool(clone);
            }
            else if (isInTowerZone && IsTowerAtMaxHeight())
            {
                ShowLocalizedMessage(LocKeys.MaxHeightReached).Forget();
                await _animationService.PlayDisappear(clone.RectTransform);
                _blockFactory.ReturnToPool(clone);
            }
            else
            {
                ShowLocalizedMessage(LocKeys.Miss).Forget();
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
                ShowLocalizedMessage(LocKeys.BlockDiscarded).Forget();
            }
            else
            {
                await _animationService.PlayDisappear(clone.RectTransform);
                _blockFactory.ReturnToPool(clone);
            }
        }

        private async UniTask ShowLocalizedMessage(string key)
        {
            string message = await _localizationService.GetStringFromCommon(key);
            _messageService.ShowMessage(message);
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
