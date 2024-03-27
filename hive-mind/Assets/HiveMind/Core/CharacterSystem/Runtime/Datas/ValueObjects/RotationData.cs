using HiveMind.CharacterSystem.Runtime.Enums;
using System;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct RotationData
    {
        #region Constants
        private const float minRotationSpeed = 0f;
        private const float maxRotationSpeed = 360f;
        #endregion

        #region Fields
        [Header("Rotation Data Fields")]
        [Range(minRotationSpeed, maxRotationSpeed)][SerializeField] private float rotationSpeed;
        [SerializeField] private RotationStyles rotationStyle;
        [SerializeField] private Space rotationSpace;
        [SerializeField] private bool isSlerp;
        #endregion

        #region Getters
        public readonly float RotationSpeed => rotationSpeed;
        public readonly RotationStyles RotationStyle => rotationStyle;
        public readonly Space RotationSpace => rotationSpace;
        public readonly bool IsSlerp => isSlerp;
        #endregion
    }
}
