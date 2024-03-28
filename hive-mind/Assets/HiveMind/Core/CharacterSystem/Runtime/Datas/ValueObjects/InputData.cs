using HiveMind.Core.CharacterSystem.Runtime.Enums;
using HiveMind.Utilities.SerializedDictionary;
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
        [SerializeField] private InputActionNamesDictionary inputActionNames;
        #endregion

        #region Getters
        public readonly InputActionAsset InputActionAsset => inputActionAsset;
        public readonly InputActionNamesDictionary InputActionNames => inputActionNames;
        #endregion
    }

    [Serializable]
    public class InputActionNamesDictionary: SerializedDictionary<InputActionNameTypes, string> { }
}
