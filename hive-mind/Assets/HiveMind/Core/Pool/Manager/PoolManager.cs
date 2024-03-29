using HiveMind.Core.Pool.Interfaces;
using HiveMind.Core.Pool.Settings;
using UnityEngine;

namespace HiveMind.Core.Pool.Manager
{
    internal sealed class PoolManager : MonoBehaviour
    {
        #region Fields
        [Header("Pool Manager Settings")]
        [SerializeField] private PoolSettings poolSettings;
        [SerializeField] private Transform[] poolParents;
        private ObjectPool[] pools;
        #endregion

        #region Getters
        internal IPoolable GetObj(string poolName, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent, bool useRotation = false, bool useScale = false, bool setParent = false)
        {
            for (int i = 0; i < pools.Length; i++)
            {
                if (pools[i].PoolName == poolName)
                    return pools[i].GetObj(pos, rot, scale, parent, useRotation, useScale, setParent);
            }

            Debug.LogError(poolName + " Cant Found");
            return null;
        }
        #endregion

        #region Core
        internal void Initialize()
        {
            Object.DontDestroyOnLoad(this);

            pools = new ObjectPool[poolSettings.Prefabs.Length];

            for (int i = 0; i < poolSettings.Prefabs.Length; i++)
            {
                ObjectPool pool = new ObjectPool(poolSettings.Prefabs[i], poolSettings.PoolNames[i], poolSettings.InitSize, poolSettings.AddSize, poolSettings.MaxSize, poolParents[i]);
                pools[i] = pool;
            }
        }
        #endregion

        #region Clears
        internal void ClearPool(string objName)
        {
            for (int i = 0; i < pools.Length; i++)
            {
                if (pools[i].PoolName == objName)
                    pools[i].Reload();
            }
        }
        internal void ClearAllPools()
        {
            for (int i = 0; i < pools.Length; i++)
                pools[i].Reload();
        }
        #endregion
    }
}
