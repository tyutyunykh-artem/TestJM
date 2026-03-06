using System.IO;
using UnityEngine;

namespace TestGame.Core.SaveSystem
{
    /// <summary>
    /// Константы для сервиса сохранений.
    /// </summary>
    public static class SaveConstants
    {
        public static readonly string SavePath = Path.Combine(Application.persistentDataPath, "TestGameSave.json");
    }
}
