using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Rotation
{
    public abstract class RotationHandler : Handler<Vector2>
    {
        #region ReadonlyFields
        protected readonly RotationData rotationData;
        protected readonly Transform transform;
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
        public override void Execute(Vector2 inputValue) => base.Execute(inputValue);
        protected override void ExecuteProcess(Vector2 inputValue) { }
        #endregion
    }
}
