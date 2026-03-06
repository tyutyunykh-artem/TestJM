using System;
using System.Collections.Generic;
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
        [Inject] private readonly TowerAreaView _towerAreaView;

        private readonly CompositeDisposable _disposables = new();
        private readonly List<BlockView> _towerBlockViews = new();

        public void Start()
        {
            _towerService.OnBlockAdded.Subscribe(OnBlockAdded).AddTo(_disposables);
            _towerService.OnBlockRemoved.Subscribe(OnBlockRemoved).AddTo(_disposables);
        }

        private void OnBlockAdded(TowerBlockEntry entry)
        {
            BlockView view = _blockFactory.CreateTowerBlock(entry.Data, _towerAreaView.TowerContainer);
            SetupBlockTransform(view);
            PositionBlock(view, _towerBlockViews.Count, entry.HorizontalOffset);
            _towerBlockViews.Add(view);
        }

        private void OnBlockRemoved(int index)
        {
            if (index < 0 || index >= _towerBlockViews.Count)
            {
                return;
            }

            BlockView removedView = _towerBlockViews[index];
            _towerBlockViews.RemoveAt(index);
            _blockFactory.ReturnToPool(removedView);

            RepositionAllBlocks();
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

        private void RepositionAllBlocks()
        {
            for (int i = 0; i < _towerBlockViews.Count; i++)
            {
                float offset = _towerService.State.Blocks[i].HorizontalOffset;
                PositionBlock(_towerBlockViews[i], i, offset);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
