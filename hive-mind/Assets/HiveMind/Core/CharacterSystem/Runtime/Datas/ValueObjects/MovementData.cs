using HiveMind.Core.CharacterSystem.Runtime.Enums;
using HiveMind.Core.Utilities.SerializedDictionary;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct MovementData
    {
        #region Fields
        [Header("Movement Data Fields")]
        [SerializeField] private MovementTypes movementType;
        [ShowIf("movementType", MovementTypes.Transform)][SerializeField] private Space movementSpace;
        [ShowIf("movementType", MovementTypes.Transform)][SerializeField] private TransformMovementStyles transformMovementStyle;
        [ShowIf("movementType", MovementTypes.Rigidbody)][SerializeField] private RigidbodyMovementStyles rigidbodyMovementStyle;
        [ShowIf("movementType", MovementTypes.Rigidbody)][SerializeField] private ForceMode forceMode;
        [SerializeField] private SpeedDictionary speeds;
        #endregion

        #region Getters
        public readonly MovementTypes MovementType => movementType;
        public readonly Space MovementSpace => movementSpace;
        public readonly TransformMovementStyles TransformMovementStyle => transformMovementStyle;
        public readonly RigidbodyMovementStyles RigidbodyMovementStyle => rigidbodyMovementStyle;
        public readonly ForceMode ForceMode => forceMode;
        public readonly SpeedDictionary Speeds => speeds;
        #endregion
    }

    [Serializable]
    public class SpeedDictionary : SerializedDictionary<MovementStatus, float> { }
}
