using UnityEngine;

namespace HiveMind.Core.Pool.Objects
{
    public class PoolParticle : PoolObject
    {
        #region Fields
        private ParticleSystem particle;
        private bool loopParticle;
        private float completeTime;
        private float destroyTime;
        #endregion

        #region Core
        public override void Initialize()
        {
            base.Initialize();

            particle = GetComponent<ParticleSystem>();
            loopParticle = particle.main.loop;
            completeTime = particle.main.duration;
        }
        public override void Spawn(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent, bool useRotation = false, bool useScale = false, bool setParent = false)
        {
            base.Spawn(pos, rot, scale, parent, useRotation, useScale, setParent);

            destroyTime = Time.time + completeTime;
        }
        public override void DeSpawn()
        {
            base.DeSpawn();
        }
        #endregion

        #region Timer
        void Update()
        {
            if (!loopParticle && InUse)
            {
                if (Time.time >= destroyTime)
                    DeSpawn();
            }
        }
        #endregion
    }
}
