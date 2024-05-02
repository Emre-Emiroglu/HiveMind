using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Movement
{
    public class TransformMovementHandler : MovementHandler
    {
        #region ReadonlyFields
        private readonly Transform transform;
        #endregion

        #region Constructor
        public TransformMovementHandler(Transform transform, MovementData movementData) : base(movementData)
        {
            this.transform = transform;
        }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public override void Execute(Vector2 inputValue, MovementStatus movementStatus) => base.Execute(inputValue, movementStatus);
        protected override void ExecuteProcess(Vector2 inputValue, MovementStatus movementStatus)
        {
            base.ExecuteProcess(inputValue, movementStatus);

            if (inputValue == Vector2.zero)
                return;

            Vector3 pos = movementData.MovementSpace == Space.World ? transform.position : transform.localPosition;
            float speed = movementData.Speeds[movementStatus];
            float time = Time.deltaTime;

            Vector3 input = new(inputValue.x, inputValue.y, 0f);
            Vector3 direction = movementData.TransformMovementStyle == TransformMovementStyles.InputBased ? input : transform.forward;
            pos += speed * time * direction;
            pos.x = Mathf.Clamp(pos.x, movementData.MinimumPositionClamp.x, movementData.MaximumPositionClamp.x);
            pos.y = Mathf.Clamp(pos.y, movementData.MinimumPositionClamp.y, movementData.MaximumPositionClamp.y);
            pos.z = Mathf.Clamp(pos.z, movementData.MinimumPositionClamp.z, movementData.MaximumPositionClamp.z);

            switch (movementData.MovementSpace)
            {
                case Space.World:
                    transform.position = pos;
                    break;
                case Space.Self:
                    transform.localPosition = pos;
                    break;
            }
        }
        #endregion
    }
}
