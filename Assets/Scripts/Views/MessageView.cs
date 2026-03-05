using TMPro;
using UnityEngine;

namespace TestGame.Views
{
    /// <summary>
    /// View полосы сообщений.
    /// </summary>
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _messageText;

        public CanvasGroup CanvasGroup => _canvasGroup;
        public TextMeshProUGUI MessageText => _messageText;

        public void SetText(string text)
        {
            _messageText.text = text;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }
    }
}
