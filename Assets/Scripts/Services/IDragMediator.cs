using R3;
using TestGame.Model;
using UnityEngine;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс для медиатора событий перетаскивания между View и Presenter.
    /// </summary>
    public interface IDragMediator
    {
        public Observable<DragStartedData> OnDragStarted { get; }
        public Observable<Vector2> OnDragUpdated { get; }
        public Observable<DragEndedData> OnDragEnded { get; }

        public void NotifyDragStarted(BlockData blockData, Vector2 screenPosition, DragSource source, int towerIndex = -1);
        public void NotifyDragUpdated(Vector2 screenPosition);
        public void NotifyDragEnded(BlockData blockData, Vector2 screenPosition, DragSource source, int towerIndex = -1);
    }
}
