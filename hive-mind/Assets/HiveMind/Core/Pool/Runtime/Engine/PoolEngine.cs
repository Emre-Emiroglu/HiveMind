using HiveMind.Core.Pool.Runtime.Interfaces;
using HiveMind.Core.Pool.Runtime.Manager;
using UnityEngine;

namespace HiveMind.Core.Pool.Runtime.Engine
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
                Debug.Log("Pool Manager Cannot found in scene!");

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
