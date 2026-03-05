using UnityEngine;
using UnityEngine.UI;

namespace TestGame.Views
{
    /// <summary>
    /// View скролла с кубами.
    /// </summary>
    public class ScrollAreaView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _content;

        public ScrollRect ScrollRect => _scrollRect;
        public RectTransform Content => _content;
    }
}
