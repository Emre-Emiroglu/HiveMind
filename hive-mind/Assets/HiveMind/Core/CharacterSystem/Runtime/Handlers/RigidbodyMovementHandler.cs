using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class RigidbodyMovementHandler : MovementHandler
    {
        #region ReadonlyFields
        private readonly Rigidbody rigidbody;
        #endregion

        #region Constructor
        public RigidbodyMovementHandler(Rigidbody rigidbody, MovementData movementData) : base(movementData)
        {
            this.rigidbody = rigidbody;
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

            Vector3 force = rigidbody.velocity;
            float speed = movementData.Speeds[movementStatus];
            float time = Time.fixedDeltaTime;
            ForceMode forceMode = movementData.ForceMode;
            force += speed * time * new Vector3(inputValue.x, 0f, inputValue.y);

            switch (movementData.RigidbodyMovementType)
            {
                case RigidbodyMovementTypes.ExplosionForce:
                    rigidbody.AddExplosionForce(force.magnitude, rigidbody.position, force.sqrMagnitude, 0f, forceMode);
                    break;
                case RigidbodyMovementTypes.Force:
                    rigidbody.AddForce(force, forceMode);
                    break;
                case RigidbodyMovementTypes.ForceAtPosition:
                    rigidbody.AddForceAtPosition(force, rigidbody.position, forceMode);
                    break;
                case RigidbodyMovementTypes.RelativeForce:
                    rigidbody.AddRelativeForce(force, forceMode);
                    break;
                case RigidbodyMovementTypes.RelativeTorque:
                    rigidbody.AddRelativeTorque(force, forceMode);
                    break;
                case RigidbodyMovementTypes.Torque:
                    rigidbody.AddTorque(force, forceMode);
                    break;
            }
        }
        #endregion
    }
}
