using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Datas.ScriptableObjects
{
    [CreateAssetMenu(fileName ="CharacterSettings", menuName = "HiveMind/CharacterSystem/CharacterSettings")]
    public sealed class CharacterSettings : ScriptableObject
    {
        #region Fields
        public InputData InputData;
        public MovementData MovementData;
        public RotationData RotationData;
        #endregion
    }
}
