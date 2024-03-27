using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Datas.ScriptableObjects
{
    [CreateAssetMenu(fileName ="CharacterSettings", menuName = "HiveMind/CharacterSystem/CharacterSettings")]
    public sealed class CharacterSettings : ScriptableObject
    {
        #region Fields
        public InputData InputData { get; set; }
        public MovementData MovementData { get; set; }
        public RotationData RotationData { get; set; }
        #endregion
    }
}
