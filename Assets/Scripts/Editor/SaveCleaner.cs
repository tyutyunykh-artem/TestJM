using System.IO;
using UnityEditor;
using UnityEngine;

namespace TestGame.Editor
{
    /// <summary>
    /// Утилита для очистки сохранений игры.
    /// </summary>
    public static class SaveCleaner
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "TestGameSave.json");

        [MenuItem("Tools/Clear Save Data")]
        public static void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log($"[{nameof(SaveCleaner)}] Save data deleted");
            }
            else
            {
                Debug.Log($"[{nameof(SaveCleaner)}] No save file found");
            }
        }
    }
}
