using TestGame.Model;
using TestGame.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace TestGame.Views
{
    /// <summary>
    /// Обработчик перетаскивания куба из башни.
    /// </summary>
    [RequireComponent(typeof(BlockView))]
    public class TowerBlockDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Inject] private readonly IDragMediator _dragMediator;

        private BlockView _blockView;
        private int _towerIndex = -1;

        private void Awake()
        {
            _blockView = GetComponent<BlockView>();
        }

        public void SetTowerIndex(int index)
        {
            _towerIndex = index;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragMediator.NotifyDragStarted(_blockView.BlockData, eventData.position, DragSource.Tower, _towerIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragMediator.NotifyDragUpdated(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragMediator.NotifyDragEnded(_blockView.BlockData, eventData.position, DragSource.Tower, _towerIndex);
        }
    }
}
