using UnityEngine;

namespace HiveMind.Core.Pool.Runtime.Settings
{
    [CreateAssetMenu(fileName ="PoolSettings", menuName ="HiveMind/Pool/PoolSettings")]
    public sealed class PoolSettings : ScriptableObject
    {
        #region Fields
        [Header("Pool Settings")]
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private string[] poolNames;
        [Range(0, 100)][SerializeField] private int initSize = 100;
        [Range(0, 100)][SerializeField] private int addSize = 50;
        [Range(100, 1000)][SerializeField] private int maxSize = 500;
        #endregion

        #region Getters
        public GameObject[] Prefabs => prefabs;
        public string[] PoolNames => poolNames;
        public int InitSize => initSize;
        public int AddSize => addSize;
        public int MaxSize => maxSize;
        #endregion
    }
}
