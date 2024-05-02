using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Rotation
{
    public class TopDownRotationHandler : RotationHandler
    {
        #region ReadonlyFields
        private readonly Camera camera;
        #endregion

        #region Constructor
        public TopDownRotationHandler(Camera camera, Transform transform, RotationData rotationData) : base(transform, rotationData)
        {
            this.camera = camera;
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

            Vector3 mousePosition = new(inputValue.x, inputValue.y, camera.nearClipPlane);
            Vector3 targetPosition = camera.ScreenToWorldPoint(mousePosition);
            Vector3 direction = targetPosition - transform.position;
            direction.z = 0f;

            float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            float speed = rotationData.RotationSpeed;
            float time = Time.deltaTime;

            switch (rotationData.RotationStyle)
            {
                case RotationStyles.Quaternion:
                    QuaternionRotate(angle, speed, time);
                    break;
                case RotationStyles.Euler:
                    EulerRotate(angle, speed, time);
                    break;
            }
        }
        private void QuaternionRotate(float angle, float speed, float time)
        {
            Quaternion startRotation = rotationData.RotationSpace == Space.World ? transform.rotation : transform.localRotation;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Quaternion lerpRotation = rotationData.IsSlerp ? Quaternion.Slerp(startRotation, targetRotation, speed * time) : Quaternion.Lerp(startRotation, targetRotation, speed * time);

            switch (rotationData.RotationSpace)
            {
                case Space.World:
                    transform.rotation = lerpRotation;
                    break;
                case Space.Self:
                    transform.localRotation = lerpRotation;
                    break;
            }
        }
        private void EulerRotate(float angle, float speed, float time)
        {
            Vector3 startRotation = rotationData.RotationSpace == Space.World ? transform.eulerAngles : transform.localEulerAngles;
            Vector3 targetRotation = new(0f, 0f, angle);
            Vector3 lerpRotation = rotationData.IsSlerp ? Vector3.Slerp(startRotation, targetRotation, speed * time) : Vector3.Lerp(startRotation, targetRotation, speed * time);

            switch (rotationData.RotationSpace)
            {
                case Space.World:
                    transform.eulerAngles = lerpRotation;
                    break;
                case Space.Self:
                    transform.localEulerAngles = lerpRotation;
                    break;
            }
        }
        #endregion
    }
}
