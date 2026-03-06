using TestGame.Services;
﻿using TestGame.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace TestGame.Views
{
    /// <summary>
    /// Обработчик перетаскивания куба из скролла.
    /// </summary>
    [RequireComponent(typeof(BlockView))]
    public class ScrollBlockDragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Inject] private readonly IDragMediator _dragMediator;

        private BlockView _blockView;
        private ScrollRect _scrollRect;
        private bool _isDragging;
        private bool _isScrolling;

        private void Awake()
        {
            _blockView = GetComponent<BlockView>();
            _scrollRect = GetComponentInParent<ScrollRect>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = false;
            _isScrolling = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 delta = eventData.delta;

            if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x) * 0.5f)
            {
                _isDragging = true;
                _scrollRect.enabled = false;
                _dragMediator.NotifyDragStarted(_blockView.BlockData, eventData.position, DragSource.Scroll);
            }
            else
            {
                _isScrolling = true;
                _scrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _dragMediator.NotifyDragUpdated(eventData.position);
            }
            else if (_isScrolling)
            {
                _scrollRect.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _scrollRect.enabled = true;
                _dragMediator.NotifyDragEnded(_blockView.BlockData, eventData.position, DragSource.Scroll);
            }
            else if (_isScrolling)
            {
                _isScrolling = false;
                _scrollRect.OnEndDrag(eventData);
            }
        }
    }
}
