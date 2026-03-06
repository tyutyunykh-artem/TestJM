using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using TestGame.Model;
using TestGame.Services;
using TestGame.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestGame.Presenters
{
    /// <summary>
    /// Презентер башни.
    /// </summary>
    public class TowerPresenter : IStartable, IDisposable
    {
        [Inject] private readonly ITowerService _towerService;
        [Inject] private readonly IBlockFactory _blockFactory;
        [Inject] private readonly IBlockAnimationService _animationService;
        [Inject] private readonly TowerAreaView _towerAreaView;
        [Inject] private readonly HoleView _holeView;

        private readonly CompositeDisposable _disposables = new();
        private readonly List<BlockView> _towerBlockViews = new();

        private RectTransform _canvasRect;

        public void Start()
        {
            _canvasRect = _towerAreaView.GetComponentInParent<Canvas>().rootCanvas.GetComponent<RectTransform>();

            _towerService.OnBlockAdded
                .Subscribe(entry => OnBlockAddedAsync(entry).Forget())
                .AddTo(_disposables);

            _towerService.OnBlockRemoved
                .Subscribe(index => OnBlockRemovedAsync(index).Forget())
                .AddTo(_disposables);

            _towerService.OnStateRestored
                .Subscribe(OnStateRestored)
                .AddTo(_disposables);
        }


        private async UniTaskVoid OnBlockAddedAsync(TowerBlockEntry entry)
        {
            int index = _towerBlockViews.Count;
            BlockView view = _blockFactory.CreateTowerBlock(entry.Data, _towerAreaView.TowerContainer);
            SetupBlockTransform(view);
            PositionBlock(view, index, entry.HorizontalOffset);
            SetTowerIndex(view, index);
            _towerBlockViews.Add(view);

            await _animationService.PlayPlacement(view.RectTransform);
        }

        private async UniTaskVoid OnBlockRemovedAsync(int index)
        {
            if (index < 0 || index >= _towerBlockViews.Count)
            {
                return;
            }

            BlockView removedView = _towerBlockViews[index];
            _towerBlockViews.RemoveAt(index);

            Vector3 holeWorldPosition = _holeView.HoleImageRect.position;
            removedView.RectTransform.SetParent(_canvasRect, true);

            UniTask fallTask = _animationService.PlayFallIntoHole(removedView.RectTransform, holeWorldPosition);
            UniTask slideTask = AnimateSlideDown();

            await UniTask.WhenAll(fallTask, slideTask);

            _blockFactory.DestroyBlock(removedView);
        }

        private async UniTask AnimateSlideDown()
        {
            List<UniTask> tasks = new List<UniTask>();

            for (int i = 0; i < _towerBlockViews.Count; i++)
            {
                float offset = _towerService.State.Blocks[i].HorizontalOffset;
                float y = i * _blockFactory.BlockHeight;
                Vector2 targetPos = new Vector2(offset, y);

                Vector2 currentPos = _towerBlockViews[i].RectTransform.anchoredPosition;
                if (Vector2.Distance(currentPos, targetPos) > 0.1f)
                {
                    tasks.Add(_animationService.PlaySlideDown(_towerBlockViews[i].RectTransform, targetPos));
                }

                SetTowerIndex(_towerBlockViews[i], i);
            }

            if (tasks.Count > 0)
            {
                await UniTask.WhenAll(tasks);
            }
        }

        private void OnStateRestored(TowerState state)
        {
            ClearAllViews();

            for (int i = 0; i < state.Blocks.Count; i++)
            {
                TowerBlockEntry entry = state.Blocks[i];
                BlockView view = _blockFactory.CreateTowerBlock(entry.Data, _towerAreaView.TowerContainer);
                SetupBlockTransform(view);
                PositionBlock(view, i, entry.HorizontalOffset);
                SetTowerIndex(view, i);
                _towerBlockViews.Add(view);
            }
        }

        private void ClearAllViews()
        {
            foreach (BlockView view in _towerBlockViews)
            {
                _blockFactory.DestroyBlock(view);
            }

            _towerBlockViews.Clear();
        }

        private void SetupBlockTransform(BlockView view)
        {
            view.RectTransform.pivot = new Vector2(0.5f, 0f);
            view.RectTransform.anchorMin = new Vector2(0.5f, 0f);
            view.RectTransform.anchorMax = new Vector2(0.5f, 0f);
            view.RectTransform.sizeDelta = new Vector2(_blockFactory.BlockWidth, _blockFactory.BlockHeight);
        }

        private void PositionBlock(BlockView view, int index, float horizontalOffset)
        {
            float y = index * _blockFactory.BlockHeight;
            view.RectTransform.anchoredPosition = new Vector2(horizontalOffset, y);
        }

        private void SetTowerIndex(BlockView view, int index)
        {
            TowerBlockDragHandler handler = view.GetComponent<TowerBlockDragHandler>();
            if (handler != null)
            {
                handler.SetTowerIndex(index);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
