using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class MovementHandler: Handler
    {
        #region ReadonlyFields
        private readonly Transform transform;
        private readonly Rigidbody rigidbody;
        private readonly MovementData movementData;
        #endregion

        #region Constructor
        public MovementHandler(Transform transform, Rigidbody rigidbody, MovementData movementData) : base()
        {
            this.transform = transform;
            this.rigidbody = rigidbody;
            this.movementData = movementData;
        }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public void Movement(Vector2 inputValue)
        {
            if (!isEnable)
                return;

            switch (movementData.MovementStyle)
            {
                case MovementStyles.Transform:
                    TransformMovement(inputValue);
                    break;
                case MovementStyles.Rigidbody:
                    RigidbodyMovement(inputValue);
                    break;
            }
        }
        private void TransformMovement(Vector2 inputValue)
        {
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
        private void RigidbodyMovement(Vector2 inputValue)
        {
            Vector3 force = rigidbody.velocity;
            float speed = movementData.WalkSpeed;
            float time = Time.fixedDeltaTime;
            ForceMode forceMode = movementData.ForceMode;

            force += speed * time * new Vector3(inputValue.x, 0f, inputValue.y);

            switch (movementData.RigidbodyMovementStyle)
            {
                case RigidbodyMovementStyles.ExplosionForce:
                    rigidbody.AddExplosionForce(force.magnitude, rigidbody.position, force.sqrMagnitude, 0f, forceMode);
                    break;
                case RigidbodyMovementStyles.Force:
                    rigidbody.AddForce(force, forceMode);
                    break;
                case RigidbodyMovementStyles.ForceAtPosition:
                    rigidbody.AddForceAtPosition(force, rigidbody.position, forceMode);
                    break;
                case RigidbodyMovementStyles.RelativeForce:
                    rigidbody.AddRelativeForce(force, forceMode);
                    break;
                case RigidbodyMovementStyles.RelativeTorque:
                    rigidbody.AddRelativeTorque(force, forceMode);
                    break;
                case RigidbodyMovementStyles.Torque:
                    rigidbody.AddTorque(force, forceMode);
                    break;
            }
        }
        #endregion
    }
}
