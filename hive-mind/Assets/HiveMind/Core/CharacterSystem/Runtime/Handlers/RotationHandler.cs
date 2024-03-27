using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Handlers
{
    public sealed class RotationHandler: Handler
    {
        #region ReadonlyFields
        private readonly Transform transform;
        private readonly RotationData rotationData;
        #endregion

        #region Constructor
        public RotationHandler(Transform transform, RotationData rotationData) : base()
        {
            this.transform = transform;
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
            Quaternion rot = rotationData.RotationSpace == Space.World ? transform.rotation : transform.localRotation;
            float speed = rotationData.RotationSpeed;
            float time = Time.deltaTime;
            float angle = Mathf.Atan2(inputValue.y, inputValue.x);
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.up);
            Quaternion lerp = rotationData.IsSlerp ? Quaternion.Slerp(rot, targetRot, speed * time) : Quaternion.Lerp(rot, targetRot, speed * time);

            switch (rotationData.RotationSpace)
            {
                case Space.World:
                    transform.rotation = lerp;
                    break;
                case Space.Self:
                    transform.localRotation = lerp;
                    break;
            }
        }
        private void EulerRotation(Vector2 inputValue)
        {
            Vector3 rot = rotationData.RotationSpace == Space.World ? transform.eulerAngles : transform.localEulerAngles;
            float speed = rotationData.RotationSpeed;
            float time = Time.deltaTime;
            float angle = Mathf.Atan2(inputValue.y, inputValue.x);
            Vector3 targetRot = new(0f, angle, 0f);
            Vector3 lerp = rotationData.IsSlerp ? Vector3.Slerp(rot, targetRot, speed * time) : Vector3.Lerp(rot, targetRot, speed * time);

            switch (rotationData.RotationSpace)
            {
                case Space.World:
                    transform.eulerAngles = lerp;
                    break;
                case Space.Self:
                    transform.localEulerAngles = lerp;
                    break;
            }
        }
        #endregion
    }
}
