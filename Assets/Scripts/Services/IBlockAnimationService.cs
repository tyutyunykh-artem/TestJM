using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TestGame.Services
{
    /// <summary>
    /// Интерфейс сервиса анимаций кубов.
    /// </summary>
    public interface IBlockAnimationService
    {
        public UniTask PlayPlacement(RectTransform block);
        public UniTask PlayDisappear(RectTransform block);
        public UniTask PlayFallIntoHole(RectTransform block, Vector3 holeWorldPosition);
        public UniTask PlaySlideDown(RectTransform block, Vector2 targetAnchoredPosition);
        public void StopAnimations(RectTransform block);
    }
}
