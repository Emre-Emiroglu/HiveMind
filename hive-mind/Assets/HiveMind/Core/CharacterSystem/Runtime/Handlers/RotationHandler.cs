using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Handlers
{
    public sealed class RotationHandler
    {
        #region Executes
        public void Rotate(Transform transform, Vector2 inputValue, RotationData rotationData)
        {
            switch (rotationData.RotationStyle)
            {
                case RotationStyles.Quaternion:
                    QuaternionRotate(transform, inputValue, rotationData);
                    break;
                case RotationStyles.Euler:
                    EulerRotate(transform, inputValue, rotationData);
                    break;
            }
        }
        private void QuaternionRotate(Transform transform, Vector2 inputValue, RotationData rotationData)
        {
        }
        private void EulerRotate(Transform transform, Vector2 inputValue, RotationData rotationData)
        {
        }
        #endregion
    }
}
