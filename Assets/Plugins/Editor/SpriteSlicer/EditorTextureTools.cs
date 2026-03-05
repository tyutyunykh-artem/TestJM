using UnityEditor;
using UnityEngine;

namespace Editor.SpriteSlicer
{
    public class EditorTextureTools
    {
        private static Texture2D _backdropTex;
        private static Texture2D _contrastTex;

        public static Texture2D ContrastTexture
        {
            get
            {
                if (_contrastTex == null)
                    _contrastTex = CreateCheckerTex(
                        new Color(0f, 0f, 0f, 0.5f),
                        new Color(1f, 1f, 1f, 0.5f));
                return _contrastTex;
            }
        }

        public static Texture2D BlankTexture => EditorGUIUtility.whiteTexture;

        public static Texture2D BackdropTexture
        {
            get
            {
                if (_backdropTex == null)
                    _backdropTex = CreateCheckerTex(
                        new Color(0.1f, 0.1f, 0.1f, 0.5f),
                        new Color(0.2f, 0.2f, 0.2f, 0.5f));
                return _backdropTex;
            }
        }

        private static Texture2D CreateCheckerTex(Color c0, Color c1)
        {
            var tex = new Texture2D(16, 16)
            {
                name = "[Generated] Checker Texture",
                hideFlags = HideFlags.DontSave
            };

            for (var y = 0; y < 8; ++y)
            for (var x = 0; x < 8; ++x)
                tex.SetPixel(x, y, c1);
            for (var y = 8; y < 16; ++y)
            for (var x = 0; x < 8; ++x)
                tex.SetPixel(x, y, c0);
            for (var y = 0; y < 8; ++y)
            for (var x = 8; x < 16; ++x)
                tex.SetPixel(x, y, c0);
            for (var y = 8; y < 16; ++y)
            for (var x = 8; x < 16; ++x)
                tex.SetPixel(x, y, c1);

            tex.Apply();
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        public static Rect DrawBackground(float ratio)
        {
            var rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = Screen.width - rect.xMin;
            rect.height = rect.width * ratio;
            GUILayout.Space(rect.height);

            if (Event.current.type != EventType.Repaint)
                return rect;

            var blank = BlankTexture;
            var check = BackdropTexture;

            // Lines above and below the texture rectangle
            GUI.color = new Color(0f, 0f, 0f, 0.2f);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin - 1, rect.width, 1f), blank);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), blank);
            GUI.color = Color.white;

            // Checker background
            DrawTiledTexture(rect, check);

            return rect;
        }

        public static void DrawTiledTexture(Rect rect, Texture tex)
        {
            GUI.BeginGroup(rect);
            
            var width = Mathf.RoundToInt(rect.width);
            var height = Mathf.RoundToInt(rect.height);

            for (var y = 0; y < height; y += tex.height)
            for (var x = 0; x < width; x += tex.width)
                GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
            
            GUI.EndGroup();
        }
    }
}