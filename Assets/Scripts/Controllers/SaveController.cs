using System;
using Cysharp.Threading.Tasks;
using R3;
using TestGame.Core.SaveSystem;
using TestGame.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestGame.Controllers
{
    /// <summary>
    /// Контроллер сохранений.
    /// </summary>
    public class SaveController : IStartable, IDisposable
    {
        private const float DebounceSeconds = 0.5f;

        [Inject] private readonly ISaveService _saveService;
        [Inject] private readonly ITowerService _towerService;

        private readonly CompositeDisposable _disposables = new();

        public void Start()
        {
            LoadAsync().Forget();

            Observable.Merge(
                    _towerService.OnBlockAdded.Select(_ => Unit.Default),
                    _towerService.OnBlockRemoved.Select(_ => Unit.Default))
                .Debounce(TimeSpan.FromSeconds(DebounceSeconds))
                .Subscribe(_ => SaveAsync().Forget())
                .AddTo(_disposables);

            Application.quitting += OnApplicationQuit;
            Application.focusChanged += OnFocusChanged;
        }

        private async UniTaskVoid LoadAsync()
        {
            if (!_saveService.HasSave())
            {
                return;
            }

            Model.TowerState loadedState = await _saveService.Load();
            if (loadedState.Blocks.Count > 0)
            {
                _towerService.RestoreState(loadedState);
            }
        }

        private async UniTaskVoid SaveAsync()
        {
            await _saveService.Save(_towerService.State);
        }

        private void OnApplicationQuit()
        {
            _saveService.Save(_towerService.State).Forget();
        }

        private void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                _saveService.Save(_towerService.State).Forget();
            }
        }

        public void Dispose()
        {
            Application.quitting -= OnApplicationQuit;
            Application.focusChanged -= OnFocusChanged;
            _disposables.Dispose();
        }
    }
}
