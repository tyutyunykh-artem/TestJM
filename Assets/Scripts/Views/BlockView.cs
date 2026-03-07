using TestGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame.Views
{
    /// <summary>
    /// View куба.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;

        private BlockData _blockData;

        public BlockData BlockData => _blockData;
        public Image Image => _image;
        public RectTransform RectTransform => _rectTransform;

        public void Initialize(BlockData data)
        {
            _blockData = data;
            _image.color = data.Color;
        }
    }
}
