using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        #region Fields
        [Header("Input Data Fields")]
        [AssetSelector][SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private string controlSchemeName;
        [SerializeField] private string actionMapName;
        [SerializeField] private string movementActionName;
        #endregion

        #region Getters
        public readonly InputActionAsset InputActionAsset => inputActionAsset;
        public readonly string ControlSchemeName => controlSchemeName;
        public readonly string ActionMapName => actionMapName;
        public readonly string MovementActionName => movementActionName;
        #endregion
    }
}
