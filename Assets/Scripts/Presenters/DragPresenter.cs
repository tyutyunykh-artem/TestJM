using R3;
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
        [Inject] private readonly ScrollAreaView _scrollAreaView;

        private readonly CompositeDisposable _disposables = new();

        private Canvas _canvas;
        private RectTransform _canvasRect;
        private BlockView _currentClone;

        public void Start()
        {
            _canvas = _scrollAreaView.GetComponentInParent<Canvas>().rootCanvas;
            _canvasRect = _canvas.GetComponent<RectTransform>();

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

            // TODO

            _blockFactory.ReturnToPool(_currentClone);
            _currentClone = null;
        }

        private void MoveCloneToScreenPosition(Vector2 screenPosition)
        {
            Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPosition, camera, out Vector2 localPoint);

            _currentClone.RectTransform.localPosition = localPoint;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
