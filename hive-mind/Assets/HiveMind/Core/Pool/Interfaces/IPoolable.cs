using UnityEngine;

namespace HiveMind.Core.Pool.Interfaces
{
    public interface IPoolable
    {
        public bool InUse { get; }
        public void Initialize();
        public void Spawn(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent, bool useRotation = false, bool useScale = false, bool setParent = false);
        public void DeSpawn();
    }
}
