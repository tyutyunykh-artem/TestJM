using TestGame.Core.Configuration;
using TestGame.Services;
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

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfiguration(builder);
            RegisterServices(builder);
        }

        private void RegisterConfiguration(IContainerBuilder builder)
        {
            builder.RegisterInstance<IGameConfig>(_gameConfig);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<ITowerService, TowerService>(Lifetime.Singleton);
        }
    }
}
