using R3;
using TestGame.Model;
using UnityEngine;

namespace TestGame.Services
{
    /// <summary>
    /// Медиатор событий перетаскивания.
    /// </summary>
    public class DragMediator : IDragMediator
    {
        private readonly Subject<DragStartedData> _onDragStarted = new();
        private readonly Subject<Vector2> _onDragUpdated = new();
        private readonly Subject<DragEndedData> _onDragEnded = new();

        public Observable<DragStartedData> OnDragStarted => _onDragStarted;
        public Observable<Vector2> OnDragUpdated => _onDragUpdated;
        public Observable<DragEndedData> OnDragEnded => _onDragEnded;

        public void NotifyDragStarted(BlockData blockData, Vector2 screenPosition)
        {
            _onDragStarted.OnNext(new DragStartedData
            {
                BlockData = blockData,
                ScreenPosition = screenPosition,
            });
        }

        public void NotifyDragUpdated(Vector2 screenPosition)
        {
            _onDragUpdated.OnNext(screenPosition);
        }

        public void NotifyDragEnded(BlockData blockData, Vector2 screenPosition)
        {
            _onDragEnded.OnNext(new DragEndedData
            {
                BlockData = blockData,
                ScreenPosition = screenPosition,
            });
        }
    }
}
