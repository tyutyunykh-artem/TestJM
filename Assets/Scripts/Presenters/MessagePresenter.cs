using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using TestGame.Services;
using TestGame.Views;
using VContainer;
using VContainer.Unity;

namespace TestGame.Presenters
{
    /// <summary>
    /// Презентер сообщений.
    /// </summary>
    public class MessagePresenter : IStartable, IDisposable
    {
        private const float FadeInDuration = 0.25f;
        private const float HoldDuration = 1f;
        private const float FadeOutDuration = 0.25f;

        [Inject] private readonly IMessageService _messageService;
        [Inject] private readonly MessageView _messageView;

        private readonly CompositeDisposable _disposables = new();

        private Sequence _currentSequence;

        public void Start()
        {
            _messageView.Hide();

            _messageService.OnMessage
                .Subscribe(text => ShowMessageAsync(text).Forget())
                .AddTo(_disposables);
        }

        private async UniTaskVoid ShowMessageAsync(string text)
        {
            _currentSequence?.Kill();

            _messageView.SetText(text);
            _messageView.CanvasGroup.alpha = 0f;

            UniTaskCompletionSource source = new UniTaskCompletionSource();

            _currentSequence = DOTween.Sequence();
            _currentSequence.Append(_messageView.CanvasGroup.DOFade(1f, FadeInDuration));
            _currentSequence.AppendInterval(HoldDuration);
            _currentSequence.Append(_messageView.CanvasGroup.DOFade(0f, FadeOutDuration));
            _currentSequence.OnComplete(() => source.TrySetResult());
            _currentSequence.OnKill(() => source.TrySetResult());

            await source.Task;
        }

        public void Dispose()
        {
            _currentSequence?.Kill();
            _disposables.Dispose();
        }
    }
}
