using HiveMind.CharacterSystem.Runtime.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct MovementData
    {
        #region Constants
        private const float minWalkSpeed = 0f;
        private const float maxWalkSpeed = 10f;
        private const float minRunSpeed = 0f;
        private const float maxRunSpeed = 10f;
        #endregion

        #region Fields
        [Header("Movement Data Fields")]
        [Range(minWalkSpeed, maxWalkSpeed)][SerializeField] private float walkSpeed;
        [Range(minRunSpeed, maxRunSpeed)][SerializeField] private float runSpeed;
        [SerializeField] private MovementStyles movementStyle;
        [SerializeField] private Space movementSpace;
        [ShowIf("movementStyle", MovementStyles.Rigidbody)][SerializeField] private ForceMode forceMode;
        #endregion

        #region Getters
        public readonly float WalkSpeed => walkSpeed;
        public readonly float RunSpeed => runSpeed;
        public readonly MovementStyles MovementStyle => movementStyle;
        public readonly Space MovementSpace => movementSpace;
        public readonly ForceMode ForceMode => forceMode;
        #endregion
    }
}
