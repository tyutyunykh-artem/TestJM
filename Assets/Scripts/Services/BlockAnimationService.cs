using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame.Services
{
    /// <summary>
    /// Сервис анимаций кубов через DOTween.
    /// </summary>
    public class BlockAnimationService : IBlockAnimationService
    {
        private const float PlacementDuration = 0.4f;
        private const float PlacementBounceHeight = 100f;
        private const float DisappearDuration = 0.3f;
        private const float FallIntoHoleDuration = 0.3f;
        private const float SlideDownDuration = 0.25f;

        public async UniTask PlayPlacement(RectTransform block)
        {
            Vector2 targetPos = block.anchoredPosition;
            block.anchoredPosition = targetPos + new Vector2(0f, PlacementBounceHeight);
            block.localScale = Vector3.one;

            UniTaskCompletionSource source = new UniTaskCompletionSource();
            block.DOAnchorPos(targetPos, PlacementDuration)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => source.TrySetResult())
                .OnKill(() => source.TrySetResult());
            await source.Task;
        }

        public async UniTask PlayDisappear(RectTransform block)
        {
            UniTaskCompletionSource source = new UniTaskCompletionSource();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(block.DOScale(Vector3.zero, DisappearDuration).SetEase(Ease.InBack));

            Image image = block.GetComponent<Image>();
            if (image != null)
            {
                sequence.Join(image.DOFade(0f, DisappearDuration));
            }

            sequence.OnComplete(() => source.TrySetResult());
            sequence.OnKill(() => source.TrySetResult());
            await source.Task;
        }

        public async UniTask PlayFallIntoHole(RectTransform block, Vector3 holeWorldPosition)
        {
            UniTaskCompletionSource source = new UniTaskCompletionSource();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(block.DOMove(holeWorldPosition, FallIntoHoleDuration).SetEase(Ease.InQuad));
            sequence.Join(block.DOScale(Vector3.zero, FallIntoHoleDuration).SetEase(Ease.InQuad));
            sequence.OnComplete(() => source.TrySetResult());
            sequence.OnKill(() => source.TrySetResult());
            await source.Task;
        }

        public async UniTask PlaySlideDown(RectTransform block, Vector2 targetAnchoredPosition)
        {
            UniTaskCompletionSource source = new UniTaskCompletionSource();
            block.DOAnchorPos(targetAnchoredPosition, SlideDownDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => source.TrySetResult())
                .OnKill(() => source.TrySetResult());
            await source.Task;
        }

        public void StopAnimations(RectTransform block)
        {
            DOTween.Kill(block);
        }
    }
}
