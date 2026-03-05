using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Core.Configuration
{
    /// <summary>
    /// Конфиг списка кубиков.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfigSO : ScriptableObject, IGameConfig
    {
        [SerializeField] private List<BlockColorEntry> _blocks = new();

        public IReadOnlyList<BlockColorEntry> Blocks => _blocks;
    }
}
