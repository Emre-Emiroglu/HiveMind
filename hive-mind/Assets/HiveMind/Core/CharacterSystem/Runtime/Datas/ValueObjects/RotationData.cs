using HiveMind.Core.CharacterSystem.Runtime.Enums;
using System;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct RotationData
    {
        #region Fields
        [Header("Rotation Data Fields")]
        [SerializeField] private RotationTypes rotationType;
        [SerializeField] private RotationStyles rotationStyle;
        [SerializeField] private Space rotationSpace;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private bool isSlerp;
        #endregion

        #region Getters
        public readonly RotationTypes RotationType => rotationType;
        public readonly RotationStyles RotationStyle => rotationStyle;
        public readonly Space RotationSpace => rotationSpace;
        public readonly float RotationSpeed => rotationSpeed;
        public readonly bool IsSlerp => isSlerp;
        #endregion
    }
}
