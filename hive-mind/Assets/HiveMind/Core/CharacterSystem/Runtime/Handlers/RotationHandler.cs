using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class RotationHandler: Handler
    {
        #region ReadonlyFields
        private readonly Transform transform;
        private readonly Camera camera;
        private readonly RotationData rotationData;
        #endregion

        #region Constructor
        public RotationHandler(Transform transform, Camera camera, RotationData rotationData) : base()
        {
            this.transform = transform;
            this.camera = camera;
            this.rotationData = rotationData;
        }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public void Rotation(Vector2 inputValue)
        {
            if (!isEnable)
                return;

            switch (rotationData.RotationStyle)
            {
                case RotationStyles.Quaternion:
                    QuaternionRotation(inputValue);
                    break;
                case RotationStyles.Euler:
                    EulerRotation(inputValue);
                    break;
            }
        }
        private void QuaternionRotation(Vector2 inputValue)
        {
            float speed = rotationData.RotationSpeed;
            float time = Time.deltaTime;
            Vector3 direction = GetDirection(inputValue);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            Quaternion startRotation = rotationData.RotationSpace == Space.World ? transform.rotation : transform.localRotation;
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
        private void EulerRotation(Vector2 inputValue)
        {
            float speed = rotationData.RotationSpeed;
            float time = Time.deltaTime;
            Vector3 direction = GetDirection(inputValue);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Vector3 targetRotation = new(0f, angle, 0f);
            Vector3 startRotation = rotationData.RotationSpace == Space.World ? transform.eulerAngles : transform.localEulerAngles;
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
        private Vector3 GetDirection(Vector2 inputValue)
        {
            Vector3 mousePosition = new(inputValue.x, inputValue.y, camera.nearClipPlane);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            return direction;
        }
        #endregion
    }
}
