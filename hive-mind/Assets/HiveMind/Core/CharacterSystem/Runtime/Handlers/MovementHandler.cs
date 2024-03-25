using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Handlers
{
    public sealed class MovementHandler
    {
        #region Executes
        public void Move(Transform transform, Vector2 inputValue, bool isRun, MovementData movementData)
        {
            switch (movementData.MovementStyle)
            {
                case MovementStyles.Transform:
                    TransformMove(transform, inputValue, isRun, movementData);
                    break;
                case MovementStyles.Rigidbody:
                    RigidbodyMove(transform, inputValue, isRun, movementData);
                    break;
            }
        }
        private void TransformMove(Transform transform, Vector2 inputValue, bool isRun, MovementData movementData)
        {
        }
        private void RigidbodyMove(Transform transform, Vector2 inputValue, bool isRun, MovementData movementData)
        {
        }
        #endregion
    }
}
