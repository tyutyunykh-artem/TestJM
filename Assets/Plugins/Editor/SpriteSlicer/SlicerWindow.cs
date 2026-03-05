using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.SpriteSlicer
{
    public class SlicerWindow : EditorWindow
    {
        private const float XOffset = 0;
        private const float VerticalOffset = 140;
        private const float MinWidth = 200;

        private Texture2D _targetTexture;
        private Texture _oldTargetTexture;

        private int _leftBorder;
        private int _rightBorder;
        private int _topBorder;
        private int _bottomBorder;

        private bool _replaceOriginal;

        [MenuItem("Tools/Editor/Sprite Slicer")]
        public static void Init()
        {
            var window = GetWindow(typeof(SlicerWindow), true, "Slicer");
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 400, 200);
            window.Show();
        }

        // ReSharper disable once InconsistentNaming
        protected void OnGUI()
        {
            _targetTexture = GetSelectedTexture();
            if (!_targetTexture)
            {
                EditorGUILayout.TextArea("Select texture in assests folder");
                return;
            }

            if (_oldTargetTexture != _targetTexture)
            {
                UpdateWindowDimensions();
                _oldTargetTexture = _targetTexture;
            }

            GUILayout.BeginVertical();
            EditorGUILayout.Space();
            _leftBorder = EditorGUILayout.IntField("Left", _leftBorder);
            _rightBorder = EditorGUILayout.IntField("Right", _rightBorder);
            _topBorder = EditorGUILayout.IntField("Top", _topBorder);
            _bottomBorder = EditorGUILayout.IntField("Bottom", _bottomBorder);
            _replaceOriginal = EditorGUILayout.Toggle("Replace original", _replaceOriginal);

            if (GUILayout.Button("Slice it!", GUILayout.Height(30f)))
                ProcessTexture();

            GUILayout.EndVertical();

            DrawPreview();
        }

        private void UpdateWindowDimensions()
        {
            var pos = position;
            pos.width = Mathf.Max(_targetTexture.width, MinWidth);
            pos.height = _targetTexture.height + VerticalOffset;
            position = pos;
            Focus();
            Repaint();
        }

        protected void OnSelectionChange() => Repaint();

        private void ProcessTexture()
        {
            var w = _targetTexture.width;
            var h = _targetTexture.height;

            if (_leftBorder <= 0f)
                _leftBorder = w / 2;
            if (_rightBorder <= 0f)
                _rightBorder = w / 2;
            if (_topBorder <= 0f)
                _topBorder = h / 2;
            if (_bottomBorder <= 0f)
                _bottomBorder = h / 2;


            var resultWidth = _leftBorder + _rightBorder;
            var resultHeight = _topBorder + _bottomBorder;
            var resultTexture = new Texture2D(resultWidth, resultHeight, _targetTexture.format, false);
            //BotLeft
            Graphics.CopyTexture(_targetTexture, 0, 0, 0, 0, _leftBorder, _bottomBorder, resultTexture,
                0, 0, 0, 0);
            //TopLeft
            Graphics.CopyTexture(_targetTexture, 0, 0, 0, h - _topBorder, _leftBorder, _topBorder,
                resultTexture, 0, 0, 0,
                _bottomBorder);
            //TopRight
            Graphics.CopyTexture(_targetTexture, 0, 0, w - _rightBorder, h - _topBorder, _rightBorder,
                _topBorder,
                resultTexture, 0, 0, _leftBorder, _bottomBorder);
            //BotRight
            Graphics.CopyTexture(_targetTexture, 0, 0, w - _rightBorder, 0, _rightBorder, _bottomBorder,
                resultTexture, 0,
                0, _leftBorder, 0);

            resultTexture.Apply(false);
            Save(resultTexture);
        }

        private void Save(Texture2D texture)
        {
            var isPng = texture.format == TextureFormat.RGBA32;
            var bytes = isPng ? texture.EncodeToPNG() : texture.EncodeToJPG();

            var originalPath = AssetDatabase.GetAssetPath(_targetTexture);
            var path = Path.Combine(Path.GetDirectoryName(originalPath)!, GetFileName(isPng));

            Debug.Log($"Saving to: {path}");

            File.WriteAllBytes(path, bytes);

            AssetDatabase.Refresh();
        }

        private string GetFileName(bool isPng)
        {
            var textureName = _targetTexture.name;
            if (!_replaceOriginal)
                textureName += "Sliced";

            return isPng ? $"{textureName}.png" : $"{textureName}.jpg";
        }

        private void DrawPreview()
        {
            GUI.color = Color.white;
            var w = _targetTexture.width;
            var h = _targetTexture.height;

            var rect = new Rect(XOffset, VerticalOffset, w, h);
            var ratio = (float) h / w;
            EditorTextureTools.DrawBackground(ratio);

            var uv = new Rect(0f, 0f, w, h);
            uv = ConvertToTexCoords(uv, w, h);
            GUI.DrawTextureWithTexCoords(rect, _targetTexture, uv, true);

            var tex = EditorTextureTools.ContrastTexture;

            if (_leftBorder > 0)
                EditorTextureTools.DrawTiledTexture(new Rect(_leftBorder, VerticalOffset, 1f, h), tex);
            if (_rightBorder > 0)
                EditorTextureTools.DrawTiledTexture(new Rect(w - _rightBorder, VerticalOffset, 1f, h),
                    tex);
            if (_topBorder > 0)
                EditorTextureTools.DrawTiledTexture(
                    new Rect(XOffset, VerticalOffset + _topBorder, w, 1f), tex);
            if (_bottomBorder > 0)
                EditorTextureTools.DrawTiledTexture(
                    new Rect(XOffset, VerticalOffset + h - _bottomBorder, w, 1f), tex);
        }

        private static Rect ConvertToTexCoords(Rect rect, int width, int height)
        {
            var final = rect;
            if (!Mathf.Approximately(width, 0f) && !Mathf.Approximately(height, 0f))
            {
                final.xMin = rect.xMin / width;
                final.xMax = rect.xMax / width;
                final.yMin = 1f - rect.yMax / height;
                final.yMax = 1f - rect.yMin / height;
            }

            return final;
        }

        private static Texture2D GetSelectedTexture()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
                return null;

            var objects = EditorUtility.CollectDependencies(Selection.objects);
            if (objects.Length == 0)
                return null;

            return objects[0] as Texture2D;
        }
    }
}