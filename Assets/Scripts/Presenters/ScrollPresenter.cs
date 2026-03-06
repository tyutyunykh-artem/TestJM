using TestGame.Core.Configuration;
using TestGame.Model;
using TestGame.Services;
using TestGame.Views;
using VContainer;
using VContainer.Unity;

namespace TestGame.Presenters
{
    /// <summary>
    /// Презентер скролла.
    /// </summary>
    public class ScrollPresenter : IStartable
    {
        [Inject] private readonly IGameConfig _gameConfig;
        [Inject] private readonly IBlockFactory _blockFactory;
        [Inject] private readonly ScrollAreaView _scrollAreaView;

        private int _nextInstanceId;

        public void Start()
        {
            FillScroll();
        }

        private void FillScroll()
        {
            for (int i = 0; i < _gameConfig.Blocks.Count; i++)
            {
                BlockColorEntry entry = _gameConfig.Blocks[i];
                BlockData data = new BlockData(entry.Id, entry.Color, _nextInstanceId++);
                _blockFactory.CreateScrollBlock(data, _scrollAreaView.Content);
            }
        }
    }
}
