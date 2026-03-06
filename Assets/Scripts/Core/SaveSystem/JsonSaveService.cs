using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TestGame.Core.Configuration;
using TestGame.Model;
using UnityEngine;
using VContainer;

namespace TestGame.Core.SaveSystem
{
    /// <summary>
    /// Сервис сохранений.
    /// </summary>
    public class JsonSaveService : ISaveService
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "TestGameSave.json");

        [Inject] private readonly IGameConfig _gameConfig;

        public async UniTask Save(TowerState state)
        {
            SaveData saveData = new SaveData
            {
                TowerBlocks = state.Blocks.Select(block => new SavedBlockEntry
                {
                    ColorId = block.Data.ColorId,
                    HorizontalOffset = block.HorizontalOffset,
                }).ToList()
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            await File.WriteAllTextAsync(SavePath, json);
        }

        public async UniTask<TowerState> Load()
        {
            TowerState towerState = new TowerState();

            if (!HasSave())
            {
                return towerState;
            }

            string json = await File.ReadAllTextAsync(SavePath);
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            if (saveData?.TowerBlocks == null)
            {
                return towerState;
            }

            int instanceId = 0;
            foreach (SavedBlockEntry saved in saveData.TowerBlocks)
            {
                BlockColorEntry colorEntry = _gameConfig.Blocks.FirstOrDefault(b => b.Id == saved.ColorId);
                BlockData blockData = new BlockData(saved.ColorId, colorEntry.Color, instanceId++);
                TowerBlockEntry entry = new TowerBlockEntry(blockData, saved.HorizontalOffset);
                towerState.AddBlock(entry);
            }

            return towerState;
        }

        public bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }
}
