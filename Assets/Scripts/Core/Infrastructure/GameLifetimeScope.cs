using TestGame.Core.Configuration;
using TestGame.Core.SaveSystem;
using TestGame.Presenters;
using TestGame.Services;
using TestGame.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestGame.Core.Infrastructure
{
    /// <summary>
    /// Lifetime scope для регистрации зависимостей игры.
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfigSO _gameConfig;
        [SerializeField] private BlockView _blockPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfiguration(builder);
            RegisterServices(builder);
            RegisterPrefabs(builder);
            RegisterViews(builder);
            RegisterPresenters(builder);
        }

        private void RegisterConfiguration(IContainerBuilder builder)
        {
            builder.RegisterInstance<IGameConfig>(_gameConfig);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<ITowerService, TowerService>(Lifetime.Singleton);
            builder.Register<ISaveService, JsonSaveService>(Lifetime.Singleton);
            builder.Register<IBlockFactory, BlockFactory>(Lifetime.Singleton);
            builder.Register<IDragMediator, DragMediator>(Lifetime.Singleton);
            builder.Register<IBlockAnimationService, BlockAnimationService>(Lifetime.Singleton);
        }

        private void RegisterPrefabs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_blockPrefab);
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<ScrollAreaView>();
            builder.RegisterComponentInHierarchy<TowerAreaView>();
            builder.RegisterComponentInHierarchy<HoleView>();
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ScrollPresenter>();
            builder.RegisterEntryPoint<DragPresenter>();
            builder.RegisterEntryPoint<TowerPresenter>();
        }
    }
}
