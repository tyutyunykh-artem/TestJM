using System;
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
            await File.WriteAllTextAsync(SaveConstants.SavePath, json);
        }

        public async UniTask<TowerState> Load()
        {
            TowerState towerState = new TowerState();

            if (!HasSave())
            {
                return towerState;
            }

            try
            {
                string json = await File.ReadAllTextAsync(SaveConstants.SavePath);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                if (saveData?.TowerBlocks == null)
                {
                    return towerState;
                }

                int instanceId = 0;
                foreach (SavedBlockEntry saved in saveData.TowerBlocks)
                {
                    BlockColorEntry colorEntry = _gameConfig.Blocks.FirstOrDefault(b => b.Id == saved.ColorId);
                    Color color = colorEntry.Id == saved.ColorId ? colorEntry.Color : Color.white;
                    BlockData blockData = new BlockData(saved.ColorId, color, instanceId++);
                    TowerBlockEntry entry = new TowerBlockEntry(blockData, saved.HorizontalOffset);
                    towerState.AddBlock(entry);
                }
            }
            catch (Exception)
            {
                return new TowerState();
            }

            return towerState;
        }

        public bool HasSave()
        {
            return File.Exists(SaveConstants.SavePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(SaveConstants.SavePath))
            {
                File.Delete(SaveConstants.SavePath);
            }
        }
    }
}
