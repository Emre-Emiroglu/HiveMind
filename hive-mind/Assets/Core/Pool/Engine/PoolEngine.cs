using HiveMind.Pool.Interfaces;
using HiveMind.Pool.Manager;
using UnityEngine;

namespace HiveMind.Pool.Engine
{
    public static class PoolEngine
    {
        #region Fields
        private static PoolManager poolManager;
        #endregion

        #region Core
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            poolManager = (PoolManager)Object.FindAnyObjectByType(typeof(PoolManager));
            
            if (poolManager == null)
                Debug.LogWarning("Pool Manager Cannot found in scene!");

            poolManager?.Initialize();
        }
        #endregion

        #region Executes
        public static IPoolable GetObj(string poolName, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent, bool useRotation = false, bool useScale = false, bool setParent = false) => poolManager?.GetObj(poolName, pos, rot, scale, parent, useRotation, useScale, setParent);
        public static void ClearPool(string poolName) => poolManager?.ClearPool(poolName);
        public static void ClearAllPools() => poolManager?.ClearAllPools();
        #endregion
    }
}
