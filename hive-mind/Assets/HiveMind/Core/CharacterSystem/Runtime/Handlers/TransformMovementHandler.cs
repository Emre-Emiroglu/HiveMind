using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class TransformMovementHandler : MovementHandler
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
        public override void Execute(Vector2 inputValue) => base.Execute(inputValue);
        protected override void ExecuteProcess(Vector2 inputValue)
        {
            base.ExecuteProcess(inputValue);

            Vector3 pos = movementData.MovementSpace == Space.World ? transform.position : transform.localPosition;
            float speed = movementData.WalkSpeed;
            float time = Time.deltaTime;

            pos += speed * time * new Vector3(inputValue.x, 0f, inputValue.y);

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
