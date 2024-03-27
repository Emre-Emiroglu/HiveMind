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
        #endregion

        #region Fields
        [Header("Movement Data Fields")]
        [Range(minWalkSpeed, maxWalkSpeed)][SerializeField] private float walkSpeed;
        [SerializeField] private MovementStyles movementStyle;
        [ShowIf("movementStyle", MovementStyles.Transform)][SerializeField] private Space movementSpace;
        [ShowIf("movementStyle", MovementStyles.Rigidbody)][SerializeField] private RigidbodyMovementStyles rigidbodyMovementStyle;
        [ShowIf("movementStyle", MovementStyles.Rigidbody)][SerializeField] private ForceMode forceMode;
        #endregion

        #region Getters
        public readonly float WalkSpeed => walkSpeed;
        public readonly MovementStyles MovementStyle => movementStyle;
        public readonly Space MovementSpace => movementSpace;
        public readonly RigidbodyMovementStyles RigidbodyMovementStyle => rigidbodyMovementStyle;
        public readonly ForceMode ForceMode => forceMode;
        #endregion
    }
}
