using UnityEngine;
using UnityEngine.UI;

namespace TestGame.Views
{
    /// <summary>
    /// View зоны дыры.
    /// </summary>
    public class HoleView : MonoBehaviour
    {
        [SerializeField] private RectTransform _holeImageRect;
        [SerializeField] private Image _holeImage;

        public RectTransform HoleImageRect => _holeImageRect;
        public Image HoleImage => _holeImage;
    }
}
