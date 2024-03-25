using HiveMind.CharacterSystem.Runtime.Enums;
using System;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        #region Constants
        #endregion

        #region Fields
        [Header("Input Data Fields")]
        [SerializeField] private InputStyles inputStyle;
        #endregion

        #region Getters
        public readonly InputStyles InputStyle => inputStyle;
        #endregion
    }
}
