using HiveMind.Core.Pool.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace HiveMind.Core.Pool.Manager
{
    internal sealed class ObjectPool
    {
        #region Fields
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly string poolName;
        private readonly int initSize;
        private readonly int addSize;
        private readonly int maxSize;
        private List<IPoolable> items;
        #endregion

        #region Getters
        public string PoolName => poolName;
        public IPoolable GetObj(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent, bool useRotation = false, bool useScale = false, bool setParent = false)
        {
            checkIsPoolFull();

            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].InUse)
                {
                    items[i].Spawn(pos, rot, scale, parent, useRotation, useScale, setParent);
                    return items[i];
                }
            }

            return null;
        }
        #endregion

        #region Core
        public ObjectPool(GameObject prefab, string poolName, int initSize, int addSize, int maxSize, Transform parent)
        {
            this.prefab = prefab;
            this.poolName = poolName;
            this.initSize = initSize;
            this.maxSize = maxSize;
            this.parent = parent;

            items = new List<IPoolable>();

            fillPool();
        }
        public void Reload()
        {
            for (int i = 0; i < items.Count; i++)
                items[i].DeSpawn();
        }
        #endregion

        #region Checks
        private bool checkIsPoolFull()
        {
            if (items.Count >= maxSize)
                return false;
            else
            {
                int newSize = items.Count + addSize;
                newSize = newSize > maxSize ? newSize : maxSize;

                for (int i = items.Count; i < newSize; i++)
                    spawn();

                return true;
            }
        }
        #endregion

        #region Spawnings
        private void fillPool()
        {
            for (int i = 0; i < initSize; ++i)
                spawn();
        }
        private void spawn()
        {
            GameObject obj = Object.Instantiate(prefab, parent);
            IPoolable tmp = obj.GetComponent<IPoolable>();

            tmp.Initialize();
            tmp.DeSpawn();

            items.Add(tmp);
        }
        #endregion
    }
}
