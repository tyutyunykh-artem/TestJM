using UnityEngine;

namespace TestGame.Views
{
    /// <summary>
    /// View зоны башни.
    /// </summary>
    public class TowerAreaView : MonoBehaviour
    {
        [SerializeField] private RectTransform _towerZoneRect;
        [SerializeField] private RectTransform _towerContainer;

        public RectTransform TowerZoneRect => _towerZoneRect;
        public RectTransform TowerContainer => _towerContainer;
    }
}
