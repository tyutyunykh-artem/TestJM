using System.IO;
using TestGame.Core.SaveSystem;
using UnityEditor;
using UnityEngine;

namespace TestGame.Editor
{
    /// <summary>
    /// Утилита для очистки сохранений игры.
    /// </summary>
    public static class SaveCleaner
    {
        [MenuItem("Tools/Clear Save Data")]
        public static void DeleteSave()
        {
            if (File.Exists(SaveConstants.SavePath))
            {
                File.Delete(SaveConstants.SavePath);
                Debug.Log($"[{nameof(SaveCleaner)}] Save data deleted");
            }
            else
            {
                Debug.Log($"[{nameof(SaveCleaner)}] No save file found");
            }
        }
    }
}
